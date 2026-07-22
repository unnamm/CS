using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Structural
{
    public class Bridge
    {
        interface IRender
        {
            void Circle(double radius);
            void Square(double line);
        }
        class Vector : IRender
        {
            public void Circle(double radius) => Console.WriteLine("draw vector");
            public void Square(double line) => Console.WriteLine("draw vector");
        }
        class Pixel : IRender
        {
            public void Circle(double radius) => Console.WriteLine("draw pixel");
            public void Square(double line) => Console.WriteLine("draw pixel");
        }

        abstract class Shape
        {
            protected IRender _render;
            protected Shape(IRender render) => _render = render;
            public abstract void Draw();
        }
        class Circle : Shape
        {
            private double _radius;
            public Circle(IRender render, double radius) : base(render) => _radius = radius;
            public override void Draw() => _render.Circle(_radius);
        }
        class Square : Shape
        {
            private double _line;
            public Square(IRender render, double radius) : base(render) => _line = radius;
            public override void Draw() => _render.Square(_line);
        }

        public static void Sample()
        {
            var circle = new Circle(new Pixel(), 5);
            circle.Draw();

            var square = new Square(new Vector(), 3);
            square.Draw();
        }
    }
}
