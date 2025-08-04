using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Create
{
    public class Builder
    {
        class Wrong1 // so many child class
        {
            interface House { }
            class GardenHouse : House { }
            class SwimHouse : House { }
        }

        class wrong2 //so many parameters
        {
            class House
            {
                public House(bool isGarden, bool isSwim) { }
            }
        }

        class House { }

        class HouseBuilder
        {
            private House _current = new();

            public HouseBuilder BuildWall() { return this; }
            public HouseBuilder BuildDoor(int doorCount) { return this; }
            public HouseBuilder BuildWindow() { return this; }
            public HouseBuilder BuildRoof(bool isColor) { return this; }
            public HouseBuilder BuildGarden() { return this; }
            public HouseBuilder BuildSwim(int swimSize) { return this; }
            public House GetHouse() => _current;
        }

        public static void Sample()
        {
            HouseBuilder builder = new();
            House swimHouse = builder
                .BuildWall()
                .BuildWindow()
                .BuildDoor(3)
                .BuildSwim(22)
                .GetHouse();

            builder = new();
            House gardenHouse = builder
                .BuildWall()
                .BuildRoof(true)
                .BuildGarden()
                .GetHouse();
        }
    }
}
