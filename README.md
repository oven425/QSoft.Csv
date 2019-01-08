# CSV Convert
Provide CSV <-> object

# Serialize

```csharp
CQData ss = new CQData() { ID = 1, Name = "Q1", Time = DateTime.Now.AddHours(-3), Test = "Test_1" };
string str = CQCSVConvert.SerializeObject(ss);
//ID,Name,Time,Test
//1,Q1,2019/1/8 下午 02:14:59,Test_1
```
```csharp
List<CQData> ss = new List<CQData>();
ss.Add(new CQData() {ID=1, Name="Q1", Time = DateTime.Now.AddHours(-3) , Test="Test_1"});
ss.Add(new CQData() { ID = 2, Name = "Q2", Time = DateTime.Now.AddHours(-2), Test = "Test_2" });
ss.Add(new CQData() { ID = 3, Name = "Q3", Time = DateTime.Now.AddHours(-1), Test = "Test_3" });
string str = CQCSVConvert.SerializeObject(ss);

//ID,Name,Time,Test
//1,Q1,2019/1/7 下午 04:18:08,Test_1
//2,Q2,2019/1/7 下午 05:18:08,Test_2
//3,Q3,2019/1/7 下午 06:18:08,Test_3
```
# Deserialize
```csharp
string csv_str = @"ID,Name,Time,Test
1,Q1,2019/1/8 下午 02:50:45,Test_1";
CQData bb = CQCSVConvert.DeserializeObject<CQData>(csv_str);

string name = bb.Name;
// Q1
```
