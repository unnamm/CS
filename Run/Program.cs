// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    try
    {
        Configuration.Yaml.DataConfig d = new(@"Yaml\DataConfig.yaml");
        
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
