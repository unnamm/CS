using System.Text.Json.Serialization;
using System.Text.Json;

namespace Lib.CS
{
    internal class JsonProcess
    {
        //file path: bin\Debug\net8.0\FILE_NAME
        public const string FILE_NAME = "data.json";

        public void MakeJson<T>(string path, T data)
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

        public T[] GetDatasFromJson<T>(string jsonPath)
        {
            var str = File.ReadAllText(jsonPath);

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter()); //MakeJson same converter

            List<T> deSerial;
            try
            {
                deSerial = JsonSerializer.Deserialize<List<T>>(str, options);
            }
            catch (Exception e)
            {
                throw new Exception("deserialize error", e);
            }

            if (deSerial == null)
                throw new Exception("return list is null");

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
            public string DataString { get; set; }
            public JsonEnum DataEnum { get; set; }
        }

        public void MakeTest()
        {
            List<JsonData> dataList = new List<JsonData>()
            {
                new JsonData(){DataInt =1, DataString = "n1", DataEnum = JsonEnum.One },
                new JsonData(){DataInt =2, DataString = "n2", DataEnum = JsonEnum.Two },
                new JsonData(){DataInt =3, DataString = "n3", DataEnum = JsonEnum.Three },
            };

            MakeJson(FILE_NAME, dataList);
            Console.WriteLine("make");
        }

        public void ReadTest()
        {
            JsonData[] dataArray = GetDatasFromJson<JsonData>(FILE_NAME);
        }

        #endregion
    }
}
