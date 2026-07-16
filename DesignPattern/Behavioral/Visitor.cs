using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Behavioral
{
    public class Visitor
    {
        class Wrong
        {
            abstract class Shape
            {
                public abstract double GetArea();
                //if add Draw(), GetPerimeter() ...
                //need to change all child class
            }
            class Circle : Shape
            {
                private double radius;
                public override double GetArea() => throw new NotImplementedException();
                //Draw();
                //GetPerimeter();
            }
            class Square : Shape
            {
                private double x;
                private double y;
                public override double GetArea() => throw new NotImplementedException();
                //Draw();
                //GetPerimeter();
            }
        }

        interface IShapeVisitor
        {
            void Visit(Circle c);
            void Visit(Square s);
        }

        interface IShape
        {
            void Accept(IShapeVisitor visitor);
        }

        class Circle : IShape
        {
            public double Radius;

            public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
        }
        class Square : IShape
        {
            public double Side;

            public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
        }
        class AreaVisitor : IShapeVisitor
        {
            public void Visit(Circle c) => Console.WriteLine($"{Math.PI * c.Radius * c.Radius}");
            public void Visit(Square s) => Console.WriteLine($"{s.Side * s.Side}");
        }
        class PerimeterVisitor : IShapeVisitor
        {
            public void Visit(Circle c) => Console.WriteLine($"{2 * Math.PI * c.Radius}");
            public void Visit(Square s) => Console.WriteLine($"{s.Side * 4}");
        }
        class Draw : IShapeVisitor //only add this
        {
            public void Visit(Circle c) => Console.WriteLine("circle draw");
            public void Visit(Square s) => Console.WriteLine("Square draw");
        }

        public static void Sample()
        {
            List<IShape> shapes = [new Circle() { Radius = 1 }, new Square() { Side = 1 }];

            IShapeVisitor sv = new Draw();
            foreach (var s in shapes)
            {
                s.Accept(sv);
            }
        }
    }
}
