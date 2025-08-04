// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var mordern = DesignPattern.Create.AbstractFactory.Sample("Mordern");
var chair = mordern.Item1;
var sofa = mordern.Item2;

Console.ReadLine();