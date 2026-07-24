namespace Configuration.Ini
{
    public class DataConfig : IniBase
    {
        public DataConfig(string filePath) : base(filePath) { }

        [IniValue("data1", "ValueInt")]
        public int Data1ValueInt { get; set; }

        [IniValue("data1", "ValueBool")]
        public bool Data1ValueBool { get; set; }

        [IniValue("data1", "ValueString")]
        public string? Data1ValueString { get; set; }

        [IniValue("data2", "ValueInt")]
        public int Data2ValueInt { get; set; }

        [IniValue("data2", "ValueBool")]
        public bool Data2ValueBool { get; set; }

        [IniValue("data2", "ValueString")]
        public string? Data2ValueString { get; set; }

        [IniValue("data3")]
        public string[]? Data3Array { get; set; }

        [IniValue("data4")]
        public int[]? Data4ValueInt { get; set; }
    }
}
