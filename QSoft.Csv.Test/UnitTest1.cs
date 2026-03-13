using Bogus;
using System.Globalization;
using System.Text;

namespace QSoft.Csv.Test
{
    public class UnitTest1
    {
        public UnitTest1()
        {

        }

        List<Data> m_Source = [];


        public Stream GenerateTestCsvFile()
        {
            m_Source = new Faker<Data>("zh_CN")
               .RuleFor(s => s.Name, f => f.Name.LastName() + f.Name.FirstName())
               .RuleFor(s => s.Age, f => f.Random.Int(5, 100))
               .RuleFor(s => s.Height, f => f.Random.Int(100, 200))
               .RuleFor(s => s.Weight, f => f.Random.Int(30, 150))
               .RuleFor(s => s.BMI, (f, s) => Math.Round(s.Weight / Math.Pow(s.Height / 100.0, 2), 2))
               .Generate(200);


            StreamWriter sw  = new(new MemoryStream(), Encoding.UTF8);
            foreach (var item in m_Source)
            {
                sw.WriteLine($"{item.Name},{item.Age},{item.Height},{item.Weight},{item.BMI}");
            }
            sw.Flush();
            sw.BaseStream.Position = 0;
            return sw.BaseStream;
        }

        public Stream GenerateTestCsvFile2()
        {
            m_Source = new Faker<Data>("zh_CN")
               .RuleFor(s => s.Name, f => f.Name.LastName() + f.Name.FirstName())
               .RuleFor(s => s.Age, f => f.Random.Int(5, 100))
               .RuleFor(s => s.Height, f => f.Random.Int(100, 200))
               .RuleFor(s => s.Weight, f => f.Random.Int(30, 150))
               .RuleFor(s => s.BMI, (f, s) => Math.Round(s.Weight / Math.Pow(s.Height / 100.0, 2), 2))
               .Generate(200);


            StreamWriter sw = new(new MemoryStream(), Encoding.UTF8);
            foreach (var item in m_Source)
            {
                sw.WriteLine($"\"{item.Name}\",\"{item.Age}\",{item.Height},{item.Weight},\"{item.BMI}\"");
            }
            sw.Flush();
            sw.BaseStream.Position = 0;
            return sw.BaseStream;
        }
        [Fact]
        public void Test1()
        {
            using var stream = GenerateTestCsvFile();
            using var reader = new QSoft.Csv.CsvReader(stream);
            int index = 0;
            while(reader.Read(out var record))
            {
                var dst = new Data
                {
                    Name = record[0].ToString(),
                    Age = record[1].ToInt() ?? 0,
                    Height = record[2].ToInt() ?? 0,
                    Weight = record[3].ToInt() ?? 0,
                    BMI = record[4].ToDouble() ?? 0,
                };

                var hr = dst == m_Source[index];
                Assert.True(hr);
                index++;
            }
        }
        [Fact]
        public void Test2()
        {
            using var stream = GenerateTestCsvFile2();
            using var reader = new QSoft.Csv.CsvReader(stream);
            int index = 0;
            while (reader.Read(out var record))
            {
                var dst = new Data
                {
                    Name = record[0].ToString(),
                    Age = record[1].ToInt() ?? 0,
                    Height = record[2].ToInt() ?? 0,
                    Weight = record[3].ToInt() ?? 0,
                    BMI = record[4].ToDouble() ?? 0,
                };

                var hr = dst == m_Source[index];
                Assert.True(hr);
                index++;
            }
        }

        public record Data
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public int Height { get; set; }
            public int Weight { set; get; }
            public double BMI { set; get; }
        }
    }
}