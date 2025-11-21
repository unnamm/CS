// See https://aka.ms/new-console-template for more information

using CommandPrompt;

Console.WriteLine("Hello, World!");

WmicCsproduct command = new WmicCsproduct();
var result = new RunCommand().RunAsync(command.Command).Result;
var dic = command.Process(result);
var str = command.ConvertString(dic);
Console.WriteLine(str);

Console.ReadLine();
