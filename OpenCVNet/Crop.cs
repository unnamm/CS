using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVNet
{
    public class Crop
    {
        public Mat Top(string path, int cropLength)
        {
            using var grayOrigin = Cv2.ImRead(path, ImreadModes.Grayscale);
            Rect rect = new Rect(0, cropLength, grayOrigin.Width, grayOrigin.Height - cropLength);
            return new Mat(grayOrigin, rect);
        }

        public Mat Top(Mat mat, int cropLength)
        {
            Rect rect = new Rect(0, cropLength, mat.Width, mat.Height - cropLength);
            return new Mat(mat, rect);
        }

        public Mat Bottom(Mat mat, int cropLength)
        {
            Rect rect = new Rect(0, 0, mat.Width, mat.Height - cropLength);
            return new Mat(mat, rect);
        }
    }
}
