
namespace Lib.CS
{
    internal class FileTxtProcess
    {
        public static string ReadText(string filePath)
        {
            IsCheckExtension(filePath);

            if (File.Exists(filePath) == false)
                throw new Exception("file is not exist");

            return File.ReadAllText(filePath);
        }

        public static void WriteText(string filePath, string text)
        {
            IsCheckExtension(filePath);
            File.WriteAllText(filePath, text); //if empty, auto make
        }

        public static void AppendText(string filePath, string text)
        {
            IsCheckExtension(filePath);
            File.AppendAllText(filePath, text); //if empty, auto make
        }

        private static bool IsCheckExtension(string filePath)
        {
            if (Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                return true;

            throw new Exception("file name need .txt");
        }

    }
}
