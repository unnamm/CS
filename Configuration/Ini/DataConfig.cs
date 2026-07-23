namespace Configuration.Ini
{
    public class DataConfig : IniBase
    {
        public int ValueInt;
        public bool ValueBool;
        public string ValueString = string.Empty;
        public string[] ValueArray = [];

        public DataConfig(string filePath) : base(filePath)
        {
            base.Get(ref ValueInt, "data1");
            base.Get(ref ValueBool, "data1");
            base.Get(ref ValueString, "data1");

            base.Get(ref ValueInt, "data2");
            base.Get(ref ValueBool, "data2");
            base.Get(ref ValueString, "data2");

            base.Get(ref ValueArray, "data3");
        }

        public void Save()
        {
            base.Set(ValueInt, "data1");
            base.Set(ValueBool, "data1");
            base.Set(ValueString, "data1");

            base.Set(ValueInt, "data2");
            base.Set(ValueBool, "data2");
            base.Set(ValueString, "data2");
        }
    }
}
