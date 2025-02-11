using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using WinMM.Properties;
using Resources = WinMM.Properties.Resources;

namespace WinMM
{
    // Token: 0x02000005 RID: 5
    public sealed class WaveIn : IDisposable
    {
        // Token: 0x06000019 RID: 25 RVA: 0x00002D24 File Offset: 0x00000F24
        public WaveIn(int deviceId)
        {
            if (deviceId >= WaveIn.DeviceCount && deviceId != -1)
            {
                throw new ArgumentOutOfRangeException("deviceId", "The Device ID specified was not within the valid range.");
            }
            this.callback = new NativeMethods.waveInProc(this.InternalCallback);
            this.deviceId = deviceId;
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00002DAC File Offset: 0x00000FAC
        ~WaveIn()
        {
            this.Dispose(false);
        }

        // Token: 0x14000001 RID: 1
        // (add) Token: 0x0600001B RID: 27 RVA: 0x00002DDC File Offset: 0x00000FDC
        // (remove) Token: 0x0600001C RID: 28 RVA: 0x00002DF8 File Offset: 0x00000FF8
        public event EventHandler<DataReadyEventArgs> DataReady;

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600001D RID: 29 RVA: 0x00002E14 File Offset: 0x00001014
        public static ReadOnlyCollection<WaveInDeviceCaps> Devices
        {
            get
            {
                return WaveIn.GetAllDeviceCaps().AsReadOnly();
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600001E RID: 30 RVA: 0x00002E20 File Offset: 0x00001020
        // (set) Token: 0x0600001F RID: 31 RVA: 0x00002E28 File Offset: 0x00001028
        public int BufferSize
        {
            get
            {
                return this.bufferSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "The value you specified is too small");
                }
                this.bufferSize = value;
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000020 RID: 32 RVA: 0x00002E48 File Offset: 0x00001048
        // (set) Token: 0x06000021 RID: 33 RVA: 0x00002E50 File Offset: 0x00001050
        public int BufferQueueSize
        {
            get
            {
                return this.bufferQueueSize;
            }
            set
            {
                if (this.bufferQueueSize <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "The value you specified is too small");
                }
                this.bufferQueueSize = value;
            }
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000022 RID: 34 RVA: 0x00002E78 File Offset: 0x00001078
        public static int DeviceCount
        {
            get
            {
                return (int)NativeMethods.waveInGetNumDevs();
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000023 RID: 35 RVA: 0x00002E80 File Offset: 0x00001080
        private static XmlDocument Manufacturers
        {
            get
            {
                if (WaveIn.manufacturers == null)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(Resources.Devices);
                    WaveIn.manufacturers = xmlDocument;
                }
                return WaveIn.manufacturers;
            }
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00002EB8 File Offset: 0x000010B8
        public void Open(WaveFormat waveFormat)
        {
            lock (this.startStopLock)
            {
                if (this.handle != null)
                {
                    throw new InvalidOperationException("The device is already open.");
                }
                NativeMethods.WAVEFORMATEX waveformatex = default(NativeMethods.WAVEFORMATEX);
                waveformatex.nAvgBytesPerSec = waveFormat.AverageBytesPerSecond;
                waveformatex.wBitsPerSample = waveFormat.BitsPerSample;
                waveformatex.nBlockAlign = waveFormat.BlockAlign;
                waveformatex.nChannels = waveFormat.Channels;
                waveformatex.wFormatTag = (short)waveFormat.FormatTag;
                waveformatex.nSamplesPerSec = waveFormat.SamplesPerSecond;
                waveformatex.cbSize = 0;
                this.recordingFormat = waveFormat.Clone();
                IntPtr tempHandle = (IntPtr)0;
                NativeMethods.Throw(NativeMethods.waveInOpen(ref tempHandle, (uint)this.deviceId, ref waveformatex, this.callback, (IntPtr)0, NativeMethods.WAVEOPENFLAGS.CALLBACK_WINDOW | NativeMethods.WAVEOPENFLAGS.CALLBACK_THREAD | NativeMethods.WAVEOPENFLAGS.WAVE_FORMAT_DIRECT), NativeMethods.ErrorSource.WaveOut);
                this.handle = new WaveInSafeHandle(tempHandle);
            }
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00002FAC File Offset: 0x000011AC
        public void Close()
        {
            lock (this.startStopLock)
            {
                if (this.handle != null)
                {
                    if (!this.handle.IsClosed && !this.handle.IsInvalid)
                    {
                        this.Stop();
                        this.handle.Close();
                    }
                    this.handle = null;
                }
            }
        }

        // Token: 0x06000026 RID: 38 RVA: 0x00003028 File Offset: 0x00001228
        public void Start()
        {
            lock (this.startStopLock)
            {
                if (this.bufferMaintainerThread != null)
                {
                    throw new InvalidOperationException("The device has already been started.");
                }
                lock (this.bufferingLock)
                {
                    this.buffering = true;
                    Monitor.Pulse(this.bufferingLock);
                }
                this.bufferMaintainerThread = new Thread(new ThreadStart(this.MaintainBuffers));
                this.bufferMaintainerThread.IsBackground = true;
                this.bufferMaintainerThread.Name = "WaveIn MaintainBuffers thread. (DeviceID = " + this.deviceId + ")";
                this.bufferMaintainerThread.Start();
                NativeMethods.Throw(NativeMethods.waveInStart(this.handle), NativeMethods.ErrorSource.WaveIn);
            }
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00003110 File Offset: 0x00001310
        public void Stop()
        {
            lock (this.startStopLock)
            {
                if (this.bufferMaintainerThread != null)
                {
                    lock (this.bufferingLock)
                    {
                        this.buffering = false;
                        Monitor.Pulse(this.bufferingLock);
                    }
                    NativeMethods.Throw(NativeMethods.waveInReset(this.handle), NativeMethods.ErrorSource.WaveIn);
                    this.bufferMaintainerThread.Join();
                    this.bufferMaintainerThread = null;
                }
            }
        }

        // Token: 0x06000028 RID: 40 RVA: 0x000031AC File Offset: 0x000013AC
        public bool SupportsFormat(WaveFormat waveFormat)
        {
            NativeMethods.WAVEFORMATEX waveformatex = default(NativeMethods.WAVEFORMATEX);
            waveformatex.nAvgBytesPerSec = waveFormat.AverageBytesPerSecond;
            waveformatex.wBitsPerSample = waveFormat.BitsPerSample;
            waveformatex.nBlockAlign = waveFormat.BlockAlign;
            waveformatex.nChannels = waveFormat.Channels;
            waveformatex.wFormatTag = (short)waveFormat.FormatTag;
            waveformatex.nSamplesPerSec = waveFormat.SamplesPerSecond;
            waveformatex.cbSize = 0;
            IntPtr intPtr = new IntPtr(0);
            NativeMethods.MMSYSERROR mmsyserror = NativeMethods.waveInOpen(ref intPtr, (uint)this.deviceId, ref waveformatex, null, (IntPtr)0, NativeMethods.WAVEOPENFLAGS.WAVE_FORMAT_QUERY);
            if (mmsyserror == NativeMethods.MMSYSERROR.MMSYSERR_NOERROR)
            {
                return true;
            }
            if (mmsyserror == NativeMethods.MMSYSERROR.WAVERR_BADFORMAT)
            {
                return false;
            }
            NativeMethods.Throw(mmsyserror, NativeMethods.ErrorSource.WaveIn);
            return false;
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00003258 File Offset: 0x00001458
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00003268 File Offset: 0x00001468
        private static WaveInDeviceCaps GetDeviceCaps(int deviceId)
        {
            NativeMethods.WAVEINCAPS waveincaps = default(NativeMethods.WAVEINCAPS);
            NativeMethods.waveInGetDevCaps(new UIntPtr((uint)deviceId), ref waveincaps, (uint)Marshal.SizeOf(waveincaps.GetType()));
            return new WaveInDeviceCaps
            {
                DeviceId = deviceId,
                Channels = (int)waveincaps.wChannels,
                DriverVersion = (int)waveincaps.vDriverVersion,
                Manufacturer = WaveIn.GetManufacturer(waveincaps.wMid),
                Name = waveincaps.szPname,
                ProductId = (int)waveincaps.wPid
            };
        }

        // Token: 0x0600002B RID: 43 RVA: 0x000032F4 File Offset: 0x000014F4
        private static string GetManufacturer(ushort manufacturerId)
        {
            XmlDocument xmlDocument = WaveIn.Manufacturers;
            XmlElement xmlElement = null;
            if (xmlDocument != null)
            {
                xmlElement = (XmlElement)xmlDocument.SelectSingleNode("/devices/manufacturer[@id='" + manufacturerId.ToString(CultureInfo.InvariantCulture) + "']");
            }
            if (xmlElement == null)
            {
                return "Unknown [" + manufacturerId + "]";
            }
            return xmlElement.GetAttribute("name");
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00003364 File Offset: 0x00001564
        private static List<WaveInDeviceCaps> GetAllDeviceCaps()
        {
            List<WaveInDeviceCaps> list = new List<WaveInDeviceCaps>();
            int deviceCount = WaveIn.DeviceCount;
            for (int i = 0; i < deviceCount; i++)
            {
                list.Add(WaveIn.GetDeviceCaps(i));
            }
            list.Add(WaveIn.GetDeviceCaps(-1));
            return list;
        }

        // Token: 0x0600002D RID: 45 RVA: 0x000033AC File Offset: 0x000015AC
        private void MaintainBuffers()
        {
            try
            {
                while (this.buffering)
                {
                    lock (this.bufferingLock)
                    {
                        while (this.bufferQueueCount >= this.bufferQueueSize && this.bufferReleaseQueue.Count == 0 && this.buffering)
                        {
                            Monitor.Wait(this.bufferingLock);
                        }
                        goto IL_5E;
                    }
                    goto IL_58;
                IL_5E:
                    if (this.bufferQueueCount < this.bufferQueueSize)
                    {
                        if (this.buffering)
                        {
                            goto IL_58;
                        }
                    }
                    while (this.bufferReleaseQueue.Count > 0)
                    {
                        this.ProcessDone();
                    }
                    continue;
                IL_58:
                    this.AddBuffer();
                    goto IL_5E;
                }
                while (this.bufferReleaseQueue.Count > 0 || this.bufferQueueCount > 0)
                {
                    lock (this.bufferingLock)
                    {
                        while (this.bufferReleaseQueue.Count == 0)
                        {
                            Monitor.Wait(this.bufferingLock, 1000);
                        }
                        goto IL_E2;
                    }
                    goto IL_DC;
                IL_E2:
                    if (this.bufferReleaseQueue.Count <= 0)
                    {
                        continue;
                    }
                IL_DC:
                    this.ProcessDone();
                    goto IL_E2;
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00003518 File Offset: 0x00001718
        private void AddBuffer()
        {
            int num = this.bufferSize * (int)this.recordingFormat.BlockAlign;
            IntPtr lpData = Marshal.AllocHGlobal(num);
            NativeMethods.WAVEHDR wavehdr = new NativeMethods.WAVEHDR
            {
                dwBufferLength = (uint)num,
                dwFlags = (NativeMethods.WAVEHDRFLAGS)0,
                lpData = lpData,
                dwUser = new IntPtr(12345)
            };
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(wavehdr));
            Marshal.StructureToPtr(wavehdr, intPtr, false);
            NativeMethods.Throw(NativeMethods.waveInPrepareHeader(this.handle, intPtr, (uint)Marshal.SizeOf(typeof(NativeMethods.WAVEHDR))), NativeMethods.ErrorSource.WaveOut);
            NativeMethods.Throw(NativeMethods.waveInAddBuffer(this.handle, intPtr, (uint)Marshal.SizeOf(typeof(NativeMethods.WAVEHDR))), NativeMethods.ErrorSource.WaveOut);
            lock (this.bufferingLock)
            {
                this.bufferQueueCount++;
                Monitor.Pulse(this.bufferingLock);
            }
        }

        // Token: 0x0600002F RID: 47 RVA: 0x00003614 File Offset: 0x00001814
        private void ProcessDone()
        {
            IntPtr intPtr;
            lock (this.bufferingLock)
            {
                intPtr = this.bufferReleaseQueue.Dequeue();
                Monitor.Pulse(this.bufferingLock);
            }
            NativeMethods.WAVEHDR wavehdr = (NativeMethods.WAVEHDR)Marshal.PtrToStructure(intPtr, typeof(NativeMethods.WAVEHDR));
            IntPtr lpData = wavehdr.lpData;
            if (wavehdr.dwBytesRecorded > 0U && this.DataReady != null)
            {
                byte[] array = new byte[wavehdr.dwBytesRecorded];
                Marshal.Copy(lpData, array, 0, (int)wavehdr.dwBytesRecorded);
                this.DataReady(this, new DataReadyEventArgs(array));
            }
            NativeMethods.Throw(NativeMethods.waveInUnprepareHeader(this.handle, intPtr, (uint)Marshal.SizeOf(typeof(NativeMethods.WAVEHDR))), NativeMethods.ErrorSource.WaveIn);
            Marshal.FreeHGlobal(lpData);
            Marshal.FreeHGlobal(intPtr);
        }

        // Token: 0x06000030 RID: 48 RVA: 0x000036F8 File Offset: 0x000018F8
        private void InternalCallback(IntPtr waveInHandle, NativeMethods.WAVEINMESSAGE message, IntPtr instance, IntPtr param1, IntPtr param2)
        {
            if (message == NativeMethods.WAVEINMESSAGE.MM_WIM_DATA)
            {
                lock (this.bufferingLock)
                {
                    this.bufferReleaseQueue.Enqueue(param1);
                    this.bufferQueueCount--;
                    Monitor.Pulse(this.bufferingLock);
                }
            }
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00003760 File Offset: 0x00001960
        private void Dispose(bool disposing)
        {
            if (this.handle != null)
            {
                this.Close();
            }
        }

        // Token: 0x0400000E RID: 14
        public const int WaveInMapperDeviceId = -1;

        // Token: 0x0400000F RID: 15
        private static XmlDocument manufacturers;

        // Token: 0x04000010 RID: 16
        private object startStopLock = new object();

        // Token: 0x04000011 RID: 17
        private object bufferingLock = new object();

        // Token: 0x04000012 RID: 18
        private WaveFormat recordingFormat;

        // Token: 0x04000013 RID: 19
        private bool buffering;

        // Token: 0x04000014 RID: 20
        private int bufferSize = 200;

        // Token: 0x04000015 RID: 21
        private int bufferQueueSize = 30;

        // Token: 0x04000016 RID: 22
        private int bufferQueueCount;

        // Token: 0x04000017 RID: 23
        private Queue<IntPtr> bufferReleaseQueue = new Queue<IntPtr>();

        // Token: 0x04000018 RID: 24
        private Thread bufferMaintainerThread;

        // Token: 0x04000019 RID: 25
        private int deviceId;

        // Token: 0x0400001A RID: 26
        private WaveInSafeHandle handle;

        // Token: 0x0400001B RID: 27
        private NativeMethods.waveInProc callback;
    }
}
