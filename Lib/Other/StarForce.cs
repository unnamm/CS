using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Other
{
    /// <summary>
    /// maplestory starforce
    /// </summary>
    internal class StarForce
    {
        int _goal;
        int _current = 15;
        int _continueFail = 0;
        Random _random = new();

        public int Run(int goal)
        {
            if (goal < 16)
            {
                throw new Exception("start 15");
            }

            if (goal > 26)
            {
                throw new Exception("max is 25");
            }

            _goal = goal;

            int play = 0;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"play num: {play}");

                var result = Force();

                if (result)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return play;
                }
                else
                {
                    play++;
                }

                _current = 15;
                _continueFail = 0;
            }
        }

        private bool Force()
        {
            while (_current < _goal)
            {
                var forceResult = IsSuccess();

                if (forceResult == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{_current}->{_current + 1}");
                    _current++;
                }
                else if (forceResult == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (_current == 15 || _current == 20)
                    {
                        Console.WriteLine($"{_current}->{_current}");
                        continue;
                    }
                    Console.WriteLine($"{_current}->{_current - 1}");
                    _current--;
                }
                else //broke
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"{_current}-> broken");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>null is broke</returns>
        private bool? IsSuccess()
        {
            if (_continueFail == 2)
            {
                _continueFail = 0;
                return true;
            }

            var chance = _random.NextDouble();

            if (chance < BrokenChance())
            {
                return null;
            }

            if (chance > ForceChance())
            {
                _continueFail = 0;
                return true;
            }
            _continueFail++;
            return false;
        }

        /// <summary>
        /// chance &lt; force : broke
        /// </summary>
        /// <returns></returns>
        private double BrokenChance()
        {
            switch (_current)
            {
                case 15:
                case 16:
                case 17:
                    return 0.021;
                case 18:
                case 19:
                    return 0.028;
                case 20:
                case 21:
                    return 0.07;
                case 22:
                    return 0.194;
                case 23:
                    return 0.294;
                case 24:
                    return 0.396;
            }
            return 0;
        }

        /// <summary>
        /// chance > force : success
        /// </summary>
        /// <returns></returns>
        private double ForceChance()
        {
            return 1 - _current switch
            {
                22 => 0.03,
                23 => 0.02,
                24 => 0.01,
                _ => 0.3,
            };
        }
    }
}
