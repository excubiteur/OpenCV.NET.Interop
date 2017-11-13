using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Test
{
    class Program
    {
        static Int32 Callback(byte[] image_bytes, int size, int height, int width)
        {
            var filename = Environment.CurrentDirectory + "\\resized.bmp";

            GCHandle handle = GCHandle.Alloc(image_bytes, GCHandleType.Pinned);
            using (var bitmap = new Bitmap(width,
                height,
                width * 4, PixelFormat.Format32bppRgb,
                Marshal.UnsafeAddrOfPinnedArrayElement(image_bytes, 0)))
            {
                bitmap.Save(filename);
            }
            handle.Free();
            return 0;
        }

        static void Main(string[] args)
        {
            OpenCV.NET.Interop.CallbackType c = new OpenCV.NET.Interop.CallbackType(Callback);
            OpenCV.NET.Interop.FrameProcessor.SetCallback(Marshal.GetFunctionPointerForDelegate(c));

            var filename = Environment.CurrentDirectory + "\\bitmap.bmp";
            if (!File.Exists(filename))
                return;

            byte[] bytes;
            using (var bitmap = new Bitmap(filename))
            {
                var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                IntPtr ptr = data.Scan0;
                int numBytes = data.Stride * bitmap.Height;
                bytes = new byte[numBytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, numBytes);
                bitmap.UnlockBits(data);
                OpenCV.NET.Interop.FrameProcessor.ProcessFrame(bytes,bitmap.Height, bitmap.Width);
            }

        }
    }
}
