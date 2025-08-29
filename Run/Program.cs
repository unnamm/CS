// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

OpenCVNet.OpenCVProcess oc = new();
var mat = oc.GetMat(@"C:\your.png");
mat = oc.Process(mat);
oc.ShowWindow(mat);

Console.ReadLine();