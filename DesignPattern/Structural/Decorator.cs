using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Structural
{
    public class Decorator
    {
        interface ICamera
        {
            void Snap();
        }

        class Camera : ICamera
        {
            public void Snap() => Console.WriteLine("run snap");
        }

        class Log : ICamera
        {
            private readonly ICamera _camera;
            public Log(ICamera camera) => _camera = camera;
            public void Snap()
            {
                Console.WriteLine("run log");
                _camera.Snap();
                Console.WriteLine("end log");
            }
        }

        class AutoSave : ICamera
        {
            private readonly ICamera _camera;
            public AutoSave(ICamera camera) => _camera = camera;
            public void Snap()
            {
                _camera.Snap();
                Console.WriteLine("run save image");
            }
        }

        public static void Sample()
        {
            ICamera device = new Camera();
            device = new AutoSave(device);
            device = new Log(device);

            device.Snap();
        }
    }
}
