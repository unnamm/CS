using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern.Structural
{
    public class Flyweight
    {
        class CharacterStyle
        {
            public string? FontFamily;
            public int FontSize;
        }

        class StyleFactory
        {
            private static readonly Dictionary<string, CharacterStyle> _cache = [];

            public static CharacterStyle GetStyle(string font, int size)
            {
                var key = $"{font}_{size}";
                if (!_cache.ContainsKey(key))
                {
                    _cache[key] = new CharacterStyle { FontFamily = font, FontSize = size };
                }
                return _cache[key];
            }
        }

        class Character
        {
            public char Symbol;
            public CharacterStyle? Style;
        }

        public static void Sample()
        {
            var style = StyleFactory.GetStyle("Arial", 12);
            var c1 = new Character { Symbol = 'A', Style = style };
            var c2 = new Character { Symbol = 'B', Style = style };

            style = StyleFactory.GetStyle("Consolas", 10);
            var c3 = new Character { Symbol = 'C', Style = style };
        }
    }
}
