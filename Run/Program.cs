// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

_ = Run();

Console.ReadLine();

async Task Run()
{
    try
    {
        Database.Database db = new Database.SQLite(@"local path .db");
        //db = new Database.MSSQL("127.0.0.1", "user", "password");
        await db.ConnectAsync(default);

        var rows = await db.ReaderAsync("SELECT * FROM TableNmae");
        var countresult = await db.ScalarAsync("SELECT COUNT(*) FROM TableName");

        var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var affect = await db.NonQueryAsync($"INSERT INTO TableName (DateTime, Description, Result) VALUES ('{now}', 'temp message', 1)");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    Console.WriteLine("end");
}
