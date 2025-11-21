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
        private readonly Mat _element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(6, 6));

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

        //erode white
        public Mat Erosion(Mat mat) => mat.Erode(_element);

        //dilate white
        public Mat Dilatation(Mat mat) => mat.Dilate(_element);

        //remove backgroud nosie
        public Mat Erode_Dilate(Mat mat) => mat.MorphologyEx(MorphTypes.Open, _element);

        //remove foreground nosie
        public Mat Dilate_Erode(Mat mat) => mat.MorphologyEx(MorphTypes.Close, _element);

        public Mat Gradient(Mat mat)
        {
            var element = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(2, 2));
            return mat.MorphologyEx(MorphTypes.Gradient, element);
        }

        public Mat Process(Mat mat)
        {
            mat = mat.CvtColor(ColorConversionCodes.BGR2GRAY);
            var v = mat.Mean();

            return mat;
        }

    }
}
