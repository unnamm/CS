using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Lib.Config
{
    internal abstract class ConfigBase
    {
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder returnedString, uint size, string filePath);

        //[DllImport("kernel32.dll")]
        //private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);

        private string _iniFile;

        public ConfigBase(string iniName)
        {
            var slnPath = tryGetSolutionDirectory();
            if (slnPath == null) //exe file
            {
                _iniFile = Path.Combine("Config", iniName);
            }
            else //edit visual studio
            {
                var folder = this.GetType().Namespace;
                if (folder == null)
                {
                    throw new Exception("no namespace");
                }
                folder = folder.Replace(".", "\\");

                //sln path \ ini folder \ ini file
                _iniFile = Path.Combine(tryGetSolutionDirectory().ToString(), folder, iniName);
            }

            if (File.Exists(_iniFile) == false)
            {
                throw new FileLoadException();
            }
        }

        //key is T value's parameter name
        protected void get<T>(ref T value, string section, [CallerArgumentExpression("value")] string key = "")
        {
            StringBuilder sb = new() { Capacity = 20 }; //string max length
            GetPrivateProfileString(section, key, "0", sb, (uint)sb.Capacity, _iniFile);

            if (typeof(T) == typeof(int))
            {
                int result = 0;
                if (int.TryParse(sb.ToString(), out result))
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
                bool result = false;
                if (bool.TryParse(sb.ToString(), out result))
                {
                    value = (T)(object)result;
                    return;
                }
            }

            throw new Exception("parse error");
        }

        private static DirectoryInfo tryGetSolutionDirectory(string currentPath = null)
        {
            var directory = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            if (directory == null)
            {
                return null;
            }

            return directory;
        }
    }
}
