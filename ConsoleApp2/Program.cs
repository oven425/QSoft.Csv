// See https://aka.ms/new-console-template for more information
using QSoft.Csv;

Console.WriteLine("Hello, World!");
var cw = new CsvWriter<int>(new MemoryStream());