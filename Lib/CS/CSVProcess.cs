using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CS
{
    internal class CSVProcess
    {
        public static void Make(string filePath, string[][] datas)
        {
            File.WriteAllLines(filePath, datas.Select(x => string.Join(',', x)));
        }

        public static string[][] GetRows(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new Exception("file is null");

            List<string[]> list = [];
            using var sr = new StreamReader(filePath);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine() ?? throw new Exception("csv line error");
                list.Add(line.Split(','));
            }

            return [.. list];
        }

        public static void Example()
        {
            const string filePath = @"D:/temp.csv";
            CSVProcess.Make(filePath, [["col1", "col2"], ["data1", "data2"]]);
            var rowData = CSVProcess.GetRows(filePath);
        }
    }
}
