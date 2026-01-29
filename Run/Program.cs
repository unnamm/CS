// See https://aka.ms/new-console-template for more information

using Communicate.Serial;
using Communicate.Tcp;
using OpenCVNet;
using OpenCvSharp;
using System.Diagnostics;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

_ = Run();

while (true)
{
    Thread.Sleep(1000);
}

async Task Run()
{
    while (true)
    {
        Tiling.Run();
    }
}
