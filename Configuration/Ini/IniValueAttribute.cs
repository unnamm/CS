using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration.Ini
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IniValueAttribute(string section, string? key = null) : Attribute
    {
        public string Section { get; } = section;
        public string? Key { get; } = key;
    }
}
