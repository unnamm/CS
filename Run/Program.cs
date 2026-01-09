// See https://aka.ms/new-console-template for more information

using Communicate;
using OpenCVNet;
using OpenCvSharp;
using System.Diagnostics;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    try
    {
        SerialCommunicate sc = new();
        sc.Connect("COM4", timeoutMilli: 1000);
        //var v = await sc.ReadAsync("\r\n");
        var v = await sc.ReadAsync(8);
    }
    catch (TimeoutException ex)
    {
        //Read Fail
    }
}
