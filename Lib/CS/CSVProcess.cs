using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CS
{
    internal class CSVProcess
    {
        /// <summary>
        /// create new file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="datas">row array</param>
        public static void Make(string filePath, string[][] datas) => File.WriteAllLines(filePath, datas.Select(x => string.Join(',', x)));

        /// <summary>
        /// get read all rows
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string[][] GetRows(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new Exception("file is null");

            List<string[]> list = [];
            using var sr = new StreamReader(filePath);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine() ?? throw new Exception("line null error");
                list.Add(line.Split(','));
            }

            return [.. list];
        }

        public static void Example()
        {
            const string filePath = @"D:/temp.csv";
            CSVProcess.Make(filePath, [["name", "value", "type"], ["name1", "value1", "type1"], ["name2", "value2", "type2"]]);
            var rowData = CSVProcess.GetRows(filePath);
        }
    }
}
