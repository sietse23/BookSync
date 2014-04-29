using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BookSync.Classes.Algemeen
{
    public class ErrorDump
    {

        // Used to Create a dump file for an exception and a screenshot.

        private static string Source;
        private static string Function;
        private static string Message;
        private static string ExtraInfo;
        private static string StackTrace;
        private static string ErrorMessage;

        public static string ErrorLogPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Log");

        public static void AddError(IntPtr Handle, string Src, string Func, Exception e, string DispError, string Extra)
        {
            Source = Src;
            Function = Func;
            Message = e.Message;
            StackTrace = e.StackTrace;
            ErrorMessage = DispError;
            ExtraInfo = Extra;

            WriteLogs(Handle);
        }

        public static void AddError(IntPtr Handle, string Src, string Func, Exception e, string DispError)
        {
            AddError(Handle, Src, Func, e, DispError, null);
        }

        public static void AddError(IntPtr Handle, string Src, string Func, Exception e)
        {
            Source = Src;
            Function = Func;
            Message = e.Message;
            StackTrace = e.StackTrace;
            ErrorMessage = null;
            ExtraInfo = null;

            WriteLogs(Handle);
        }

        public static void AddMessage(string LogMessage)
        {
            Source = "";
            Function = "";
            Message = LogMessage;
            StackTrace = "";
            ErrorMessage = null;
            ExtraInfo = null;

            WriteLogs(IntPtr.Zero);
        }

        private static void WriteLogs(IntPtr Handle)
        {
            if (!Directory.Exists(ErrorLogPath))
            {
                Directory.CreateDirectory(ErrorLogPath);
            }

            int volgnr = GetVolgnr();
            int errparmnr = 1;
            DateTime timestamp = DateTime.Now;

            if (ErrorMessage != null && Handle != System.IntPtr.Zero)
            {
                CaptureScreen(Handle, volgnr);
            }

            Globals.Log(Message);

            try
            {
                StreamWriter mainLog = File.AppendText(string.Format("{0}\\mainLog.txt", ErrorLogPath));
                mainLog.WriteLine("--------------");
                mainLog.WriteLine(string.Format( "{0} - {1} ({2} {3}", timestamp, volgnr,Source,Message));
                mainLog.Close();

                StreamWriter log = File.CreateText(string.Format("{0}\\errorLog{1}.txt", ErrorLogPath, volgnr.ToString()));
                log.WriteLine(String.Format("Date      : {0}", timestamp));
                log.WriteLine(String.Format("Filename  : {0}", Source));
                log.WriteLine(String.Format("Function  : {0}", Function));
                log.WriteLine(String.Format("Message   : {0}", Message));
                // log.WriteLine("Connection: " + NevDB.connCount.ToString());
                if (ExtraInfo != null)
                {
                    log.WriteLine(string.Format("Extra   : {0}", ExtraInfo));
                }
                log.WriteLine(string.Format("Stacktrace: {0}", StackTrace));

                log.Close();
            }
            catch { }


            DisplayError();
        }

        private static void CaptureScreen(IntPtr Handle, int Volgnr)
        {
            ScreenCapture sc = new ScreenCapture();

            sc.CaptureWindowToFile(Handle, string.Format("{0}\\window{1}.jpg", ErrorLogPath, Volgnr), ImageFormat.Jpeg);
            sc.CaptureScreenToFile(string.Format("{0}\\screen{1}.jpg", ErrorLogPath, Volgnr), ImageFormat.Jpeg);
        }

        private static int GetVolgnr()
        {
            int i = 1;

            string filename = string.Format("{0}\\errorlog1.txt", ErrorLogPath);

            while (File.Exists(filename))
            {
                i++;
                filename = string.Format("{0}\\errorlog{1}.txt", ErrorLogPath, i);
            }

            return i;
        }

        private static void DisplayError()
        {            
            string sError = Message;

            if (ErrorMessage == null) return;

            if (ErrorMessage == "")
            {
                ErrorMessage = sError;
            }

            MessageBox.Show(ErrorMessage, Globals.Current.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

        }
    }

    public class ScreenCapture
    {
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }
        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }
        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }
        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }
    }
}
