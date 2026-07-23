// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    try
    {
        var c = new Configuration.Ini.DataConfig(@"Ini\DataConfig.ini");

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
