using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Create
{
    public class AbstractFactory
    {
        public interface Chair { }
        public interface Sofa { }

        class ModernChair : Chair { }
        class ModernSofa : Sofa { }

        class VictorianChair : Chair { }
        class VictorianSofa : Sofa { }

        interface Factory
        {
            Chair CreateChair();
            Sofa CreateSofa();
        }

        class VictorianFactory : Factory
        {
            public Chair CreateChair() => new VictorianChair();
            public Sofa CreateSofa() => new VictorianSofa();
        }

        class ModernFactory : Factory
        {
            public Chair CreateChair() => new ModernChair();
            public Sofa CreateSofa() => new ModernSofa();
        }

        public static (Chair, Sofa) Sample(string clientRequest)
        {
            Factory? factory = null;

            if (clientRequest == "Mordern")
            {
                factory = new ModernFactory();
            }
            if (clientRequest == "Victorian")
            {
                factory = new VictorianFactory();
            }

            if (factory == null)
            {
                throw new NullReferenceException();
            }

            return (factory.CreateChair(), factory.CreateSofa());
        }
    }
}
