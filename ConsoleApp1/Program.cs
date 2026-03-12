// See https://aka.ms/new-console-template for more information

using QSoft.Csv;
using System.Text;


GenerateTestCsvFile("data.csv");
using var reader = new CsvReader("data.csv");
List<People> peopleList = [];
while (reader.TryReadRecord(out var columns))
{
    var people = new People
    {
        Name = columns[0].ToString(),
        Age = columns[1].ToInt() ?? 0,
        Height = columns[2].ToInt() ?? 0
    };
    peopleList.Add(people);
}
foreach (var people in peopleList)
{
    Console.WriteLine(people.Name);
}
//try
//{
//    var aa = CsvSerializer.Deserialize<People>("data.csv", x =>
//    {
//        var people = new People();
//        people.Name = x[0].ToString();
//        people.Age = x[1].ToInt() ?? 0;
//        people.Height = x[2].ToInt() ?? 0;
//        return people;
//    });
//    foreach(var oo in aa)
//    {

//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Error: {ex.Message}");
//}
Console.WriteLine("");

static void GenerateTestCsvFile(string filePath)
{
    var lines = new[]
    {
        "\"na\r\nme\",age,\"height\",\"weight\"",
        "\"Alice\r\n艾莉絲\",\"25\",\"165\",\"55\"",
        "\"Bob\",\"30\",\"180\",\"75\"",
        "\"Charlie\",\"28\",\"175\",\"70\"",
        "\"Diana\",\"26\",\"160\",\"52\"",
        "\"Evan\",\"32\",\"185\",\"80\"",
        "\"Fiona\",\"24\",\"162\",\"50\"",
        "\"George\",\"29\",\"178\",\"72\"",
        "\"Hannah\",\"27\",\"168\",\"58\"",
        "\"Ivan\",\"31\",\"182\",\"78\"",
        "\"Julia\",\"25\",\"164\",\"54\""
    };

    File.WriteAllLines(filePath, lines, Encoding.UTF8);
    Console.WriteLine($"✓ 已生成 {filePath}，包含 {lines.Length} 行資料");
}

public class People
{
    public string Name { set; get; }
    public int Age { set; get; }
    public int Height { set; get; }
}