// See https://aka.ms/new-console-template for more information
using QSoft.Csv;

Console.WriteLine("Hello, World!");

var cw = new CsvWriter<PP>();



public class CsvWriter<PP>
{

}


public class PP
{
    public string Name { set; get; } = "";
    public int Age { set; get; } = 0;
}