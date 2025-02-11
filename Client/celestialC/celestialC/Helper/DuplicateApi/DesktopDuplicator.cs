using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using celestialC.Helper.Services.compression;
using SharpDX.Direct3D;

namespace DesktopDuplication
{
    public class DesktopDuplicator
    {
        private static OutputDuplication _deskDupl;
        private static Device _device;
        private static OutputDescription _outputDesc;
        private static Texture2DDescription _textureDesc;
        private static Texture2D _desktopImageTexture;
        private static OutputDuplicateFrameInformation _frameInfo;
        private static int _currentMonitor;

        public void Dispose()
        {
            _desktopImageTexture?.Dispose();
            _device?.Dispose();
            _deskDupl?.Dispose();

            //important because of null checks
            _deskDupl = null;
            _device = null;
            _desktopImageTexture = null;
        }


        public static void Initialize(int monitor)
        {
            const int graphicsCardAdapter = 0;
            Adapter1 adapter;
            try
            {
                adapter = new Factory1().GetAdapter1(graphicsCardAdapter);
            }
            catch (SharpDXException)
            {
                throw new DesktopDuplicationException("Could not find the specified graphics card adapter.");
            }

            Output output;
            using (adapter)
            {
                _device = new Device(adapter);
                using (var multiThread = _device.QueryInterface<DeviceMultithread>())
                {
                    multiThread.SetMultithreadProtected(true);
                }
                try
                {
                    output = adapter.GetOutput(monitor);
                }
                catch (SharpDXException)
                {
                    throw new DesktopDuplicationException("Could not find the specified output device.");
                }
            }

            using (output)
            using (var output1 = output.QueryInterface<Output1>())
            {
                _outputDesc = output.Description;
                _textureDesc = new Texture2DDescription
                {
                    CpuAccessFlags = CpuAccessFlags.Read,
                    BindFlags = BindFlags.None,
                    Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                    Width = _outputDesc.DesktopBounds.GetWidth(),
                    Height = _outputDesc.DesktopBounds.GetHeight(),
                    OptionFlags = ResourceOptionFlags.None,
                    MipLevels = 1,
                    ArraySize = 1,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Staging
                };

                try
                {
                    _deskDupl = output1.DuplicateOutput(_device);
                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode.Code == SharpDX.DXGI.ResultCode.NotCurrentlyAvailable.Result.Code)
                    {
                        throw new DesktopDuplicationException(
                            "There is already the maximum number of applications using the Desktop Duplication API running, please close one of the applications and try again.");
                    }
                }
            }

            _currentMonitor = monitor;
        }

        public RemoteDesktopDataInfo CaptureScreen(IStreamCodec streamCodec)
        {
            if (!RetrieveFrame())
                return null;

            // Get the desktop capture texture
            var mapSource = _device.ImmediateContext.MapSubresource(_desktopImageTexture, 0, MapMode.Read,
                            MapFlags.None);

            try
            {
                if (_frameInfo.TotalMetadataBufferSize > 0)
                {
                    int movedRegionsLength;
                    OutputDuplicateMoveRectangle[] movedRectangles =
                        new OutputDuplicateMoveRectangle[_frameInfo.TotalMetadataBufferSize];
                    _deskDupl.GetFrameMoveRects(movedRectangles.Length, movedRectangles, out movedRegionsLength);
                    var movedRegions =
                        new MovedRegion[movedRegionsLength / Marshal.SizeOf(typeof(OutputDuplicateMoveRectangle))];

                    for (int i = 0; i < movedRegions.Length; i++)
                    {
                        var moveRectangle = movedRectangles[i];
                        movedRegions[i] = new MovedRegion
                        {
                            Source = new Point(moveRectangle.SourcePoint.X, moveRectangle.SourcePoint.Y),
                            Destination =
                                new Rectangle(moveRectangle.DestinationRect.Left,
                                    moveRectangle.DestinationRect.Top,
                                    moveRectangle.DestinationRect.GetWidth(),
                                    moveRectangle.DestinationRect.GetHeight())
                        };
                    }

                    int dirtyRegionsLength;
                    var dirtyRectangles = new RawRectangle[_frameInfo.TotalMetadataBufferSize - movedRegionsLength];
                    _deskDupl.GetFrameDirtyRects(dirtyRectangles.Length, dirtyRectangles, out dirtyRegionsLength);
                    var updatedAreas = new Rectangle[dirtyRegionsLength / Marshal.SizeOf(typeof(Rectangle))];

                    for (int i = 0; i < updatedAreas.Length; i++)
                    {
                        var dirtyRectangle = dirtyRectangles[i];
                        updatedAreas[i] = new Rectangle(dirtyRectangle.Left, dirtyRectangle.Top,
                            dirtyRectangle.GetWidth(), dirtyRectangle.GetHeight());
                    }

                    return streamCodec.CodeImage(mapSource.DataPointer, updatedAreas, movedRegions,
                        new Size(_outputDesc.DesktopBounds.GetWidth(), _outputDesc.DesktopBounds.GetHeight()),
                        PixelFormat.Format32bppArgb);
                }
                else
                {
                    return streamCodec.CodeImage(mapSource.DataPointer,
                        new Rectangle(0, 0, _outputDesc.DesktopBounds.GetWidth(), _outputDesc.DesktopBounds.GetHeight()),
                        new Size(_outputDesc.DesktopBounds.GetWidth(), _outputDesc.DesktopBounds.GetHeight()),
                        PixelFormat.Format32bppArgb);
                }

            }
            finally
            {
                _device.ImmediateContext.UnmapSubresource(_desktopImageTexture, 0);
                ReleaseFrame();
            }
        }

        private bool RetrieveFrame()
        {
            if (_desktopImageTexture == null)
                _desktopImageTexture = new Texture2D(_device, _textureDesc);
            SharpDX.DXGI.Resource desktopResource = null;
            _frameInfo = new OutputDuplicateFrameInformation();

            try
            {
                _deskDupl?.AcquireNextFrame(500, out _frameInfo, out desktopResource);
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Code == SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                {
                    return false;
                }
                if (ex.ResultCode.Failure)
                {
                    throw new DesktopDuplicationException("Failed to acquire next frame.");
                }
            }

            using (var tempTexture = desktopResource?.QueryInterface<Texture2D>())
                _device?.ImmediateContext.CopyResource(tempTexture, _desktopImageTexture);
            desktopResource.Dispose();
            return true;
        }
        private void ReleaseFrame()
        {
            try
            {
                _deskDupl?.ReleaseFrame();
           //     if (_deskDupl == null) Initialize(_currentMonitor);
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Failure)
                {
                    throw ex;
                }
            }
        }
    }
}
