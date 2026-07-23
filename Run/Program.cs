// See https://aka.ms/new-console-template for more information

using Communicate.Abstract;
using Communicate.Serial;
using Communicate.Tcp;

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    try
    {
        var tcp = new TcpCallback("ip", 1234);
        await tcp.ConnectAsync(default);

        ICallback callback = tcp;
        callback.DataReceived += x => Console.WriteLine(x.Length);
        await Task.Delay(1000);
        tcp.Dispose();

        var serial = new SerialCallback(3, "COM2");
        serial.Connect();

        callback = serial;
        callback.DataReceived += x => Console.WriteLine(x.Length);
        await Task.Delay(1000);
        serial.Dispose();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
