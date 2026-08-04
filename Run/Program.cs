// See https://aka.ms/new-console-template for more information

using Hosting;

Console.WriteLine("Hello, World!");
try
{
    await Main.Sample();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.WriteLine("goodbye, world!");
