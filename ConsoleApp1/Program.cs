// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

try
{
    var wait = int.Parse(args[0]);
    Thread.Sleep(wait);
}
finally { }