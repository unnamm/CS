using System.Collections.ObjectModel;

namespace Lib.CS
{
    internal class LogProcess
    {
        public ObservableCollection<string> LogList { get; set; } = []; //print list ui
        private int _maxLine; //print list max line
        private string _fileName = string.Empty;
        private string _folderPath = string.Empty;
        private DateTime _currentTime;

        /// <summary>
        /// make folder, set folder path
        /// </summary>
        /// <param name="folderPath">path + foldername</param>
        /// <param name="maxLine">LogList max count</param>
        public void Initialize(string folderPath, int maxLine = 100)
        {
            _maxLine = maxLine;
            _folderPath = folderPath;
            _currentTime = DateTime.Now;

            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }

            if (folderPath.Contains(':')) //folder full path
            {
                _fileName = Path.Combine(folderPath, _currentTime.ToString("yyyy-MM-dd") + ".txt");
            }
            else //current run folder
            {
                _fileName = Path.Combine(Directory.GetCurrentDirectory(), folderPath, _currentTime.ToString("yyyy-MM-dd") + ".txt");
            }
        }

        /// <summary>
        /// print textfile, print LogList array
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            if (_currentTime.Day != DateTime.Now.Day) //check next day
            {
                Initialize(_folderPath, _maxLine);
            }

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
