// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
_ = Run();
Console.ReadLine();

async Task Run()
{
    try
    {
        Communicate.Http.Client c = new();
        var response = await c.SendAsync("https");

        var r = await response.Content.ReadAsStringAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
