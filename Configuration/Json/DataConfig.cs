using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Configuration.Json
{
    public class DataConfig : JsonBase
    {
        public DataConfig(string filePath) : base(filePath) { }

        [JsonPropertyName("integer")]
        public int DataInt { get; set; }

        [JsonPropertyName("string")]
        public string? DataString { get; set; }

        [JsonPropertyName("double")]
        public double DataDouble { get; set; }

        [JsonPropertyName("bool")]
        public bool DataBool { get; set; }

        [JsonPropertyName("array")]
        public int[]? Array { get; set; }

        [JsonPropertyName("pair")]
        public Dictionary<int, string>? Dic { get; set; }
    }
}
