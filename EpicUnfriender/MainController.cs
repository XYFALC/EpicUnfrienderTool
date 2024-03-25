﻿using System;
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
            while (timeOut < 5)
            {
                MainController controller = new MainController();
                controller.TakeScreenshot();
                Mat inputImage = CvInvoke.Imread("capture.jpg");
                Mat templateImage = CvInvoke.Imread(targetImage);
                Mat templateOutput = new Mat();
                CvInvoke.MatchTemplate(inputImage, templateImage, templateOutput, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
                double minVal = 0.0d;
                double maxVal = 0.0d;
                Point minLoc = new Point();
                Point maxLoc = new Point();
                Point midLoc = new Point();

                CvInvoke.MinMaxLoc(templateOutput, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                CvInvoke.Threshold(templateOutput, templateOutput, 0.85, 1, Emgu.CV.CvEnum.ThresholdType.ToZero);
                midLoc.X = maxLoc.X + (templateImage.Width / 2);
                midLoc.Y = maxLoc.Y + (templateImage.Height / 2);

                if (maxVal > 0.85)
                {
                    Console.WriteLine("Image " + targetImage + " found.");
                    return midLoc;
                }
                timeOut++;
                Console.WriteLine("Image " + targetImage + " not found attempt "+ timeOut);
                Thread.Sleep(100);
            }
            throw new TimeoutException("Timeout occurred while searching for image: " + targetImage);

        }


        public static double CheckImageCoordinates(string targetImage) {
            MainController controller = new MainController();
            controller.TakeScreenshot();
            Mat inputImage = CvInvoke.Imread("capture.jpg");
            Mat templateImage = CvInvoke.Imread(targetImage);
            Mat templateOutput = new Mat();
            CvInvoke.MatchTemplate(inputImage, templateImage, templateOutput, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
            double minVal = 0.0d;
            double maxVal = 0.0d;
            Point minLoc = new Point();
            Point maxLoc = new Point();
            CvInvoke.MinMaxLoc(templateOutput, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            CvInvoke.Threshold(templateOutput, templateOutput, 0.85, 1, Emgu.CV.CvEnum.ThresholdType.ToZero);
            return maxVal;
        }

    }

}
