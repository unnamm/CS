using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

//if you use C# 10 under, how to use CallerArgumentExpressionAttribute
//namespace System.Runtime.CompilerServices
//{
//    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
//    internal sealed class CallerArgumentExpressionAttribute : Attribute
//    {
//        public CallerArgumentExpressionAttribute(string parameterName)
//        {
//            ParameterName = parameterName;
//        }

//        public string ParameterName { get; }
//    }
//}

namespace Lib.Config
{
    internal abstract class ConfigBase
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

        /// <summary>
        /// write array
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

        private readonly int _capacity;
        private readonly string _iniFile;

        public ConfigBase(string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
            {
                string iniName = this.GetType().Name + ".ini"; //cs same ini name
                _iniFile = Path.Combine("Config", iniName);
            }
            else
            {
                if (filePath.Contains(".ini") == false)
                {
                    throw new Exception("need .ini");
                }

                _iniFile = filePath;
            }

            if (File.Exists(_iniFile) == false)
            {
                throw new FileLoadException("file is not exist");
            }

            _capacity = File.ReadAllText(_iniFile).Length;
        }

        /// <summary>
        /// read options in file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">set value</param>
        /// <param name="section"></param>
        /// <param name="key">value param name</param>
        /// <exception cref="NotImplementedException"></exception>
        protected void Get<T>(ref T value, string section, [CallerArgumentExpression("value")] string key = "")
        {
            if (typeof(T) == typeof(string[]))
            {
                var buffer = new byte[_capacity];
                var length = GetPrivateProfileSection(section, buffer, (uint)buffer.Length, _iniFile);

                var reads = new byte[length];
                Array.Copy(buffer, reads, reads.Length);

                var datas = Encoding.UTF8.GetString(reads).Split('\0');
                Array.Resize(ref datas, datas.Length - 1);

                value = (T)(object)datas;
                return;
            }

            StringBuilder sb = new() { Capacity = _capacity }; //string max length
            _ = GetPrivateProfileString(section, key, "0", sb, (uint)sb.Capacity, _iniFile);

            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(sb.ToString(), out int result))
                {
                    value = (T)(object)result;
                    return;
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (double.TryParse(sb.ToString(), out double result))
                {
                    value = (T)(object)result;
                    return;
                }
            }

            if (typeof(T) == typeof(string))
            {
                value = (T)(object)sb.ToString();
                return;
            }

            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(sb.ToString(), out bool result))
                {
                    value = (T)(object)result;
                    return;
                }
            }

            throw new NotImplementedException("other parse");
        }

        /// <summary>
        /// write options in file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">save param</param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        protected void Set<T>(T value, string section, [CallerArgumentExpression("value")] string key = "")
        {
            if (value == null)
            {
                throw new NullReferenceException();
            }

            if (value is System.Collections.IEnumerable)
            {
                throw new NotImplementedException("array not implement");
            }

            WritePrivateProfileString(section, key, value.ToString()!, _iniFile);
        }
    }
}
