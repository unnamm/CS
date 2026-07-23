// See https://aka.ms/new-console-template for more information

using Configuration.Yaml;

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    try
    {
        var config = new DataConfig(@"Yaml\DataConfig.yaml");
        config.Save();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
