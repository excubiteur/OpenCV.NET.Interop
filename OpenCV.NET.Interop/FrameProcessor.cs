using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV.NET.Interop
{
    public delegate Int32 CallbackType([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]byte[] image_bytes, Int32 size, Int32 height, Int32 width);

    public class FrameProcessor
    {
        [DllImport("OpenCVFrameProcessor.dll")]
        public static extern Int16 ProcessFrame(byte[] bytes, Int32 height, Int32 width);

        [DllImport("OpenCVFrameProcessor.dll")]
        public static extern void SetCallback(IntPtr callback);

    }
}
