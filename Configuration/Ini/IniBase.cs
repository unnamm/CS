using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Configuration.Ini
{
    public abstract class IniBase
    {
        /// <summary>
        /// read value
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(
            string section, string key, string defaultValue, StringBuilder returnedString, uint size, string filePath);

        /// <summary>
        /// read array
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileSection(string IpAppName, byte[] IpPairValues, uint nSize, string IpFileName);

        /// <summary>
        /// write value
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);

        private readonly string _iniFile;

        public IniBase(string filePath)
        {
            _iniFile = filePath;

            if (File.Exists(_iniFile) == false)
                throw new FileLoadException("file is not exist");

            Load();
        }

        /// <summary>
        /// read every [IniValue] property in the derived type from the ini file
        /// </summary>
        public void Load()
        {
            foreach (var property in GetType().GetProperties())
            {
                var attr = property.GetCustomAttribute<IniValueAttribute>();
                if (attr == null)
                {
                    continue;
                }

                var key = attr.Key ?? property.Name;
                var obj = Get(property.PropertyType, attr.Section, key);
                property.SetValue(this, obj);
            }
        }

        /// <summary>
        /// write every [IniValue] property in the derived type to the ini file
        /// </summary>
        public void Save()
        {
            foreach (var property in GetType().GetProperties())
            {
                var attr = property.GetCustomAttribute<IniValueAttribute>();
                if (attr == null)
                {
                    continue;
                }

                var key = attr.Key ?? property.Name;
                Set(property.PropertyType, property.GetValue(this), attr.Section, key);
            }
        }

        /// <summary>
        /// read options in file
        /// </summary>
        /// <param name="type">value type</param>
        /// <param name="section"></param>
        /// <param name="key">value param name</param>
        /// <exception cref="NotImplementedException"></exception>
        protected object? Get(Type type, string section, string key)
        {
            var capacity = File.ReadAllText(_iniFile).Length;

            if (type.IsArray)
            {
                var elementType = type.GetElementType()!;

                var buffer = new byte[capacity];
                var length = GetPrivateProfileSection(section, buffer, (uint)buffer.Length, _iniFile);

                var reads = new byte[length];
                Array.Copy(buffer, reads, reads.Length);

                var datas = Encoding.UTF8.GetString(reads).Split('\0');
                Array.Resize(ref datas, datas.Length - 1);

                var result = Array.CreateInstance(elementType, datas.Length);
                for (int i = 0; i < datas.Length; i++)
                {
                    result.SetValue(ParseValue(elementType, datas[i]), i);
                }

                return result;
            }

            StringBuilder sb = new();
            _ = GetPrivateProfileString(section, key, "0", sb, (uint)capacity, _iniFile);

            return ParseValue(type, sb.ToString());
        }

        /// <summary>
        /// convert the raw ini text of a single value into the requested scalar type
        /// </summary>
        private static object ParseValue(Type type, string text)
        {
            if (type == typeof(int))
                return int.TryParse(text, out int result) ? result : 0;

            if (type == typeof(double))
                return double.TryParse(text, out double result) ? result : 0.0;

            if (type == typeof(string))
                return text;

            if (type == typeof(bool))
                return bool.TryParse(text, out bool result) && result;

            throw new NotImplementedException("other parse");
        }

        /// <summary>
        /// write options in file
        /// </summary>
        /// <param name="type">value type</param>
        /// <param name="value">save param</param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        protected void Set(Type type, object? value, string section, string key)
        {
            if (value == null)
                throw new NullReferenceException();

            if (type.IsArray)
            {
                var lines = ((System.Collections.IEnumerable)value).Cast<object>().Select(x => x?.ToString() ?? "").ToArray();
                SetSection(section, lines);
                return;
            }

            WritePrivateProfileString(section, key, value.ToString()!, _iniFile);
        }

        /// <summary>
        /// replace every line under [section] with the given lines, keeping the rest of the file untouched.
        /// WritePrivateProfileSection can't be used here: it merges by "key=value" identity, so key-less raw
        /// lines (no '=') never get replaced, only appended.
        /// </summary>
        private void SetSection(string section, string[] lines)
        {
            var content = File.ReadAllText(_iniFile);
            var newline = content.Contains("\r\n") ? "\r\n" : "\n";
            var fileLines = content.Replace("\r\n", "\n").Split('\n').ToList();

            var header = $"[{section}]";
            var start = fileLines.FindIndex(l => l.Trim().Equals(header, StringComparison.OrdinalIgnoreCase));

            if (start == -1)
            {
                fileLines.Add(header);
                fileLines.AddRange(lines);
            }
            else
            {
                var end = start + 1;
                while (end < fileLines.Count && fileLines[end].Trim().Length > 0 && !fileLines[end].TrimStart().StartsWith('['))
                {
                    end++;
                }

                fileLines.RemoveRange(start + 1, end - (start + 1));
                fileLines.InsertRange(start + 1, lines);
            }

            File.WriteAllText(_iniFile, string.Join(newline, fileLines));
        }
    }
}
