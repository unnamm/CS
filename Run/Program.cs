// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    try
    {
        Configuration.Ini.DataConfig d = new(@"Ini\DataConfig.ini");
        d.Data4ValueInt = [1, 2, 3, 4, 5];
        d.Save();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
