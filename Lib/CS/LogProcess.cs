using System.Collections.ObjectModel;

namespace Lib.CS
{
    internal class LogProcess
    {
        public ObservableCollection<string> LogList { get; set; } = []; //print other list
        private int _maxLine; //print list max line
        private string _fileName = string.Empty;
        private string _folderName = string.Empty;

        /// <summary>
        /// make folder, set folder path
        /// </summary>
        /// <param name="folderPath">path + foldername</param>
        /// <param name="maxLine">LogList max count</param>
        public void Initialize(string folderPath, int maxLine = 100)
        {
            _folderName = folderPath;
            _maxLine = maxLine;

            if (Directory.Exists(_folderName) == false)
            {
                Directory.CreateDirectory(_folderName);
            }

            if (folderPath.Contains(':')) //folder full path
            {
                _fileName = Path.Combine(_folderName, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            }
            else //current run folder
            {
                _fileName = Path.Combine(Directory.GetCurrentDirectory(), _folderName, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            }
        }

        /// <summary>
        /// print textfile, print LogList array
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            var mes = $"[{DateTime.Now:HH:mm:ss.f}]: {message}";
            LogList.Insert(0, mes); //insert first

            if (LogList.Count > _maxLine)
            {
                LogList.RemoveAt(LogList.Count - 1);
            }

            File.AppendAllText(_fileName, mes + "\n");
        }
    }
}
