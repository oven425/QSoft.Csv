namespace QSoft.Csv.Test
{
    public class UnitTest1
    {
        StreamWriter writer = new StreamWriter("test.csv");
        public UnitTest1()
        {

        }
        public void GenerateTestCsvFile(string filename)
        {

        }
        [Fact]
        public void Test1()
        {

        }

        public class Data
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public int Height { get; set; }
        }
    }
}