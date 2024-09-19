using System.Text.Json.Serialization;
using System.Text.Json;

namespace Lib.CS
{
    internal class JsonProcess
    {
        /// <summary>
        /// write json
        /// </summary>
        /// <typeparam name="T">Data class</typeparam>
        /// <param name="path">file path</param>
        /// <param name="data"></param>
        /// <exception cref="Exception"></exception>
        public static void MakeJson<T>(string path, IEnumerable<T> data)
        {
            if (!path.Contains(".json"))
            {
                throw new Exception("file name error");
            }

            var options = new JsonSerializerOptions();
            options.WriteIndented = true; //pretty indented line
            options.Converters.Add(new JsonStringEnumConverter()); //enum -> string

            string jsonString;

            try
            {
                jsonString = JsonSerializer.Serialize(data, options);
            }
            catch (Exception ex)
            {
                throw new Exception("Serialize error", ex);
            }

            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// read json
        /// </summary>
        /// <typeparam name="T">data class</typeparam>
        /// <param name="jsonPath"></param>
        /// <returns>data class array</returns>
        /// <exception cref="Exception"></exception>
        public static T[] GetDatasFromJson<T>(string jsonPath)
        {
            var str = File.ReadAllText(jsonPath);

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter()); //MakeJson same converter

            List<T> deSerial;
            try
            {
                deSerial = (JsonSerializer.Deserialize<List<T>>(str, options)) ?? throw new Exception("Deserialize null");
            }
            catch (Exception e)
            {
                throw new Exception("deserialize error", e);
            }

            return deSerial.ToArray();
        }

        #region Test

        public enum JsonEnum
        {
            None, One, Two, Three
        }

        //need get set
        public class JsonData
        {
            public int DataInt { get; set; }
            public string DataString { get; set; } = string.Empty;
            public JsonEnum DataEnum { get; set; }
        }

        //file path: bin\Debug\net8.0\FILE_NAME
        public const string FILE_NAME = "data.json";

        public static void MakeTest()
        {
            List<JsonData> dataList = new List<JsonData>()
            {
                new JsonData(){DataInt =1, DataString = "n1", DataEnum = JsonEnum.One },
                new JsonData(){DataInt =2, DataString = "n2", DataEnum = JsonEnum.Two },
                new JsonData(){DataInt =3, DataString = "n3", DataEnum = JsonEnum.Three },
            };

            MakeJson(FILE_NAME, dataList);
        }

        public static void ReadTest()
        {
            JsonData[] dataArray = GetDatasFromJson<JsonData>(FILE_NAME);
        }

        #endregion
    }
}
