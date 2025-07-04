// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

Run();

Console.ReadLine();

void Run()
{
    var buff = DrawingNet.MakeImage.CreateImageBytes(640, 480);
    OpenCVNet.OpenCVProcess p = new();
    p.ShowWindow(buff);
}
