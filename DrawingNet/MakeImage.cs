using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace DrawingNet
{
    public class MakeImage
    {
        [SupportedOSPlatform("windows")]
        public static byte[] CreateImageBytes(int width, int height)
        {
            using var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);

            g.Clear(Color.AliceBlue);
            g.DrawString("Hello", new Font("Consolas", 16), Brushes.Black, new PointF(10, 40));

            using var stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }
    }
}
