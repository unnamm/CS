using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.OpenCV
{
    class OpenCVProcess
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
    }
}
