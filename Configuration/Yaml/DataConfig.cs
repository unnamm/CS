using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace Configuration.Yaml
{
    public class DataConfig : YamlBase
    {
        public DataConfig(string yamlPath) : base(yamlPath) { }

        [YamlMember(Alias = "int")]
        public int Data1 { get; set; }

        [YamlMember(Alias = "double")]
        public double Data2 { get; set; }

        [YamlMember(Alias = "string1")]
        public string? String1 { get; set; }

        [YamlMember(Alias = "string2")]
        public string? String2 { get; set; }

        [YamlMember(Alias = "string3")]
        public string? String3 { get; set; }

        [YamlMember(Alias = "string4")]
        public string? String4 { get; set; }

        [YamlMember(Alias = "array")]
        public int[]? Array { get; set; }

        [YamlMember(Alias = "array2")]
        public int[]? Array2 { get; set; }

        [YamlMember(Alias = "null1")]
        public object? Null1 { get; set; }

        [YamlMember(Alias = "null2")]
        public object? Null2 { get; set; }

        [YamlMember(Alias = "null3")]
        public object? Null3 { get; set; }

        [YamlMember(Alias = "mapping1")]
        public Dictionary<string, int>? Mapping3 { get; set; }

        [YamlMember(Alias = "mapping2")]
        public Data? Mapping2 { get; set; }

        [YamlMember(Alias = "list")]
        public Data[]? List1 { get; set; }
    }

    public class Data
    {
        [YamlMember(Alias = "data1")]
        public int Data1 { get; set; }

        [YamlMember(Alias = "data2")]
        public int Data2 { get; set; }

        [YamlMember(Alias = "data3")]
        public int Data3 { get; set; }
    }
}
