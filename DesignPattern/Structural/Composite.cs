using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Structural
{
    public class Composite
    {
        interface ICamera
        {
            void Snap();
        }

        class RealCamera : ICamera
        {
            public void Snap() => Console.WriteLine("snap");
        }

        class CameraGroup : ICamera
        {
            private readonly List<ICamera> _list = [];

            public void Add(params ICamera[] items) => _list.AddRange(items);

            public void Snap()
            {
                foreach (var c in _list)
                {
                    c.Snap();
                }
            }
        }

        public static void Sample()
        {
            var group1 = new CameraGroup();
            group1.Add(new RealCamera());
            group1.Add(new RealCamera(), new RealCamera());

            var group2 = new CameraGroup();
            group2.Add(new RealCamera(), new RealCamera());
            group1.Add(group2);

            ICamera c = group1;
            c.Snap();
            c = group2;
            c.Snap();
        }
    }
}
