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
        private int _current;
        private int _continueFail;
        private bool _isHide;
        private Random _random = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="isHideForceLog"></param>
        /// <returns>broke count</returns>
        /// <exception cref="Exception"></exception>
        public int Run(int goal, bool isHideForceLog = false)
        {
            if (goal < 16)
            {
                throw new Exception("start 15");
            }

            if (goal > 25)
            {
                throw new Exception("max is 25");
            }

            _isHide = isHideForceLog;
            _current = 15;
            _continueFail = 0;
            int brokeCount = 0;

            while (goal != _current)
            {
                var result = Force();

                if (result == false)
                {
                    brokeCount++;
                    _current = 15;
                    _continueFail = 0;
                    WriteLine($"{_current}-> broke: {brokeCount}", ConsoleColor.Red);
                }
            }

            WriteLine($"try count: {brokeCount + 1}", ConsoleColor.Gray);
            return brokeCount;
        }

        private void WriteLine(string message, ConsoleColor color)
        {
            if (_isHide == true)
                return;

            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>false: broke</returns>
        private bool Force()
        {
            var forceResult = IsSuccess();

            if (forceResult == true)
            {
                WriteLine($"{_current}->{++_current}", ConsoleColor.Green);
            }
            else if (forceResult == false)
            {
                if (_current == 15 || _current == 20)
                {
                    WriteLine($"{_current}->{_current}", ConsoleColor.Red);
                }
                else
                {
                    WriteLine($"{_current}->{--_current}", ConsoleColor.Red);
                }
            }
            else
            {
                return false;
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

            if (chance < BrokeChance())
            {
                return null;
            }

            if (chance > ForceChance())
            {
                _continueFail = 0;
                return true;
            }

            if (_current == 15 || _current == 20)
            {
                _continueFail = 0;
                return false;
            }

            _continueFail++;
            return false;
        }

        /// <summary>
        /// chance &lt; force : broke
        /// </summary>
        /// <returns></returns>
        private double BrokeChance()
        {
            if (_current < 15)
                return 1;

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
