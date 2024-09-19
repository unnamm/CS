
namespace Lib.CS
{
    internal class FileTxtProcess
    {
        public static string ReadText(string file)
        {
            if (file.Contains(".txt") == false)
            {
                throw new Exception();
            }

            if (File.Exists(file) == false)
            {
                return string.Empty;
            }

            return File.ReadAllText(file);
        }

        public static void WriteText(string file, string text)
        {
            if (file.Contains(".txt") == false)
            {
                throw new Exception();
            }

            File.WriteAllText(file, text); //if empty, auto make
        }

        public static void AppendText(string file, string text)
        {
            if (file.Contains(".txt") == false)
            {
                throw new Exception();
            }

            File.AppendAllText(file, text); //auto make
        }
    }
}
