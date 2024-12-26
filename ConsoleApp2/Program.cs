// See https://aka.ms/new-console-template for more information
using QSoft.Csv;

Console.WriteLine("Hello, World!");


var cw = new CsvWriter<int>(new MemoryStream());

[CsvContextAttribute]
public class dd
{
    public int Id { get; set; }
    public string ddd { set; get; }
}