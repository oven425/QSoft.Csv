// See https://aka.ms/new-console-template for more information

using QSoft.Csv;
using System.Text;


GenerateTestCsvFile("data.csv");
using var reader = new CsvReader("data.csv");
while (reader.Readline(out var line))
{
    var columns = reader.SplitColumn(line);
    foreach (var range in columns)
    {
        var columnValue = line[range];
        int.TryParse(columnValue, out int numericValue);
        string category = columnValue.ToString();
    }
}
static void GenerateTestCsvFile(string filePath)
{
    var lines = new[]
    {
        "\"name\",\"age\",\"height\",\"weight\"",
        "\"Alice\",\"25\",\"165\",\"55\"",
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