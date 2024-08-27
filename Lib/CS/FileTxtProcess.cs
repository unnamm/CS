
namespace Lib.CS
{
    internal class FileTxtProcess
    {
        public void AddTextAndCreate(string file, string add)
        {
            if (File.Exists(file) == false)
            {
                var v = File.Create(file);
                v.Dispose(); //file.create is filestream. need dispose
            }

            var read = File.ReadAllText(file);
            File.WriteAllText(file, read + add); //auto enter line
        }
    }
}
