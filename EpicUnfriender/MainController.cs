using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV;
using System.Threading;
using System.Threading.Tasks;




namespace EpicUnfriender
{

    public class MainController
    {
        public void TakeScreenshot()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = "capture.jpg";
            string filePath = System.IO.Path.Combine(currentDirectory, fileName);
            Bitmap captureBitmap = new Bitmap(1920, 1080, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
            captureBitmap.Save(filePath, ImageFormat.Jpeg);
        }
    }

    public class MouseController
    {
        //Import SetCursorPos functions
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        // Import mouse_event function from user32.dll
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

        // Define mouse event constants
        const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        const uint MOUSEEVENTF_LEFTUP = 0x04;

        public static void LeftClick()
        {
            // Perform left mouse button down event
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);

            // Perform left mouse button up event
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

    }

    public class ImageDetection
    {
        //Find the targetImage withtin the screenshot(capture.jpg) and return the x,y coordinates of the targetImage.
        public static Point GetImageCoordinates(string targetImage)
        {
            int timeOut = 0;
            Mat inputImage = CvInvoke.Imread("capture.jpg");
            Mat templateImage = CvInvoke.Imread(targetImage);
            Mat templateOutput = new Mat();
            CvInvoke.MatchTemplate(inputImage, templateImage, templateOutput, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
            double minVal = 0.0d;
            double maxVal = 0.0d;
            Point minLoc = new Point();
            Point maxLoc = new Point();
            while (timeOut < 5)
            {
                CvInvoke.MinMaxLoc(templateOutput, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                CvInvoke.Threshold(templateOutput, templateOutput, 0.85, 1, Emgu.CV.CvEnum.ThresholdType.ToZero);
                if (maxVal > 0.85)
                {
                    break;
                }
                timeOut++;
                Console.WriteLine("Image " + targetImage + " not found.");
                Thread.Sleep(1000);
                MainController controller = new MainController();

                // Call the TakeScreenshot method
                controller.TakeScreenshot();

                //Probleem is hij moet nieuwe screenshot maken na eerste timeout. 
            }
            if (timeOut == 5)
            {
                throw new TimeoutException("Timeout occurred while searching for image: " + targetImage);
            }
            Console.WriteLine("Image " + targetImage + " found.");
            return maxLoc;
        }
    }

}
