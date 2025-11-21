using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPrompt
{
    public class WmicCsproduct
    {
        public readonly string Command = "wmic csproduct";

        public Dictionary<string, string> Process(string str)
        {
            string[] lines = str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var index = Array.FindIndex(lines, x => x.Contains(Command));

            string headerLine = lines[index + 1].Replace("\r", "");
            string valueLine = lines[index + 2].Replace("\r", "");

            // 열 시작 위치 계산
            List<int> columnStarts = new List<int>();
            for (int i = 0; i < headerLine.Length; i++)
            {
                if ((i == 0 || headerLine[i - 1] == ' ') && headerLine[i] != ' ')
                {
                    columnStarts.Add(i);
                }
            }
            columnStarts.Add(headerLine.Length); // 마지막 열 끝

            var dict = new Dictionary<string, string>();
            for (int i = 0; i < columnStarts.Count - 1; i++)
            {
                int start = columnStarts[i];
                int length = columnStarts[i + 1] - start;
                string key = headerLine.Substring(start, length).Trim();
                string value = valueLine.Length >= start + length
                    ? valueLine.Substring(start, length).Trim()
                    : valueLine.Substring(start).Trim();
                dict[key] = value;
            }

            return dict;
        }

        public string ConvertString(Dictionary<string, string> dic)
        {
            string temp = string.Empty;
            foreach (var pair in dic)
            {
                temp += $"{pair.Key}:{pair.Value}" + Environment.NewLine;
            }
            return temp;
        }
    }
}
