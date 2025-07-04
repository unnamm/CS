using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVNet
{
    public class OpenCVProcess
    {
        /// <summary>
        /// get mat from file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Mat GetMat(string filePath) => new Mat(filePath);

        /// <summary>
        /// show popup window image from mat
        /// </summary>
        /// <param name="mat"></param>
        public void ShowWindow(Mat mat)
        {
            Cv2.ImShow("title", mat);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        public void ShowWindow(byte[] imageData)
        {
            var mat = Mat.FromImageData(imageData);
            ShowWindow(mat);
        }

        /// <summary>
        /// find rect count from image
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public int SearchRectCount(Mat mat)
        {
            using var grayScale = mat.CvtColor(ColorConversionCodes.BGR2GRAY);
            using var threshold = grayScale.Threshold(60, 250, ThresholdTypes.Binary);

            Cv2.FindContours(threshold, out var contours, out var hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            return contours.Select(Cv2.BoundingRect).Count();
        }

        public Point[][] SearchContours(Mat mat)
        {
            using var grayScale = mat.CvtColor(ColorConversionCodes.BGR2GRAY);
            using var threshold = grayScale.Threshold(60, 250, ThresholdTypes.Binary);

            Cv2.FindContours(threshold, out var contours, out var hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            return contours;
        }

        /// <summary>
        /// mark rect from image
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="contours"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public void MarkRect(ref Mat mat, Point[][] contours, Scalar scalar)
        {
            foreach (var p in contours)
            {
                var rect = Cv2.BoundingRect(p);
                Cv2.Rectangle(mat, rect, scalar, 2);
            }
        }

        public void T()
        {
            var min = 15.0;
            var max = 40.0;
            var minp = (min + 273.15) * 100;
            var maxp = (max + 273.15) * 100;
            var alpha = 255.0 / (maxp - minp);
            var beta = -minp * alpha;

            var data = Cv2.ImRead(@"C:\Users\rkddk\Downloads\pipe_img_sample\20250413_173244.png", ImreadModes.Color);
            data = data.Resize(new Size(1920, 1024));
            Mat oa = new();
            Cv2.ConvertScaleAbs(data, oa);
            Mat oa2 = new();
            Cv2.ApplyColorMap(oa, oa2, ColormapTypes.Jet);
            Cv2.ImShow("win", oa2);
            Cv2.WaitKey(0);
        }
    }
}
