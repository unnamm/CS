// See https://aka.ms/new-console-template for more information

using Communicate.Serial;
using OpenCVNet;
using OpenCvSharp;
using System.Diagnostics;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    SerialCommunicateExample.RunEvent();
}
