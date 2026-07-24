// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    try
    {
        Configuration.Json.DataConfig d = new(@"Json\DataConfig.json");
        d.DataInt = 332;
        d.Save();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
