// See https://aka.ms/new-console-template for more information

using Hosting;

Console.WriteLine("Hello, World!");
_ = Run();
Console.ReadLine();

async Task Run()
{
    try
    {
        await Main.Sample();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
