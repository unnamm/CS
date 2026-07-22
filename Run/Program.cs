// See https://aka.ms/new-console-template for more information

using Communicate.Serial;
using Communicate.Tcp;
using DesignPattern.Behavioral;
using OpenCVNet;
using OpenCvSharp;
using System.Diagnostics;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    DesignPattern.Structural.Decorator.Sample();

    Console.WriteLine("end");
}
