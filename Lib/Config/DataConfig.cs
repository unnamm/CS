
namespace Lib.Config
{
    internal class DataConfig : ConfigBase
    {
        public int ValueInt;
        public string ValueString = string.Empty;
        public bool ValueBool;

        public DataConfig()
        {
            base.get(ref ValueInt, "data1");
            base.get(ref ValueString, "data1");
            base.get(ref ValueBool, "data1");

            base.get(ref ValueInt, "data2");
            base.get(ref ValueString, "data2");
            base.get(ref ValueBool, "data2");
        }

        public static void Run()
        {
            DataConfig dc = new DataConfig();
        }
    }
}
