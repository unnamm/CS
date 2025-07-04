// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

Run();

Console.ReadLine();

void Run()
{
    OpenCVNet.OpenCVProcess p = new();
    p.MakeImage(640, 480);
}
