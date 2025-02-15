﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

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

            //var slnPath = tryGetSolutionDirectory();
            //if (slnPath == null) //exe file in output folder
            //{
            //    _iniFile = Path.Combine("Config", iniName);
            //}
            //else //edit visual studio
            //{
            //    var folder = this.GetType().Namespace;
            //    if (folder == null)
            //    {
            //        throw new Exception("no namespace");
            //    }
            //    folder = folder.Replace(".", "\\");

            //    //sln path \ ini folder \ ini file
            //    _iniFile = Path.Combine(slnPath.ToString(), folder, iniName);
            //}

            if (File.Exists(_iniFile) == false)
            {
                throw new FileLoadException("file is not exist");
            }

            _capacity = File.ReadAllText(_iniFile).Length;
        }

        //key is T value's parameter name
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

        protected void Set<T>(T value, string section, [CallerArgumentExpression("value")] string key = "")
        {
            if (value == null)
            {
                throw new NullReferenceException("value is null");
            }

            if (value is System.Collections.IEnumerable)
            {
                throw new NotImplementedException("write array not implement");
            }

            WritePrivateProfileString(section, key, value.ToString()!, _iniFile);
        }

        //search sln path
        //private static DirectoryInfo? tryGetSolutionDirectory(string? currentPath = null)
        //{
        //    var directory = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
        //    while (directory != null && directory.GetFiles("*.sln").Length == 0)
        //    {
        //        directory = directory.Parent;
        //    }

        //    if (directory == null)
        //    {
        //        return null;
        //    }

        //    return directory;
        //}
    }
}
