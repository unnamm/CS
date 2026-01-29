using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVNet
{
    public class Tiling
    {
        public static void Run()
        {
            Console.Write("folder path:");
            var folderPath = Console.ReadLine()!;
            var files = Directory.GetFiles(folderPath, "*.jpg");

            if (files.Count() == 0)
            {
                Console.WriteLine("num = 0, exit");
                return;
            }

            var folderInfo = new DirectoryInfo(folderPath);
            var parent = Directory.GetParent(folderPath);
            var tileFolder = Path.Combine(parent!.FullName, $"{folderInfo.Name}Tile");

            if (!Directory.Exists(tileFolder))
            {
                Directory.CreateDirectory(tileFolder);
            }

            int count = 0;
            foreach (var file in files)
            {
                using Mat mat = Cv2.ImRead(file, ImreadModes.Grayscale);
                using Mat rotate = new();
                Cv2.Rotate(mat, rotate, RotateFlags.Rotate90Clockwise);
                using Mat half = new(rotate, new Rect(0, 0, rotate.Width / 2 + 50, rotate.Height));

                const int TILECOUNT = 3;
                const int SIZE = 512;
                int width = half.Width / TILECOUNT;
                for (int i = 0; i < TILECOUNT; i++)
                {
                    var fileName = Path.Combine(tileFolder, $"{i}_" + Path.GetFileName(file));

                    using Mat tile = new(half, new Rect(i * width, 0, width, half.Height));
                    using var resize = tile.Resize(new Size(SIZE, SIZE));
                    resize.SaveImage(fileName);
                }
                Console.WriteLine($"{++count}/{files.Count()}");
            }
            Console.WriteLine("end");
        }
    }
}
