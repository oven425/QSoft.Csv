using QCSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_CSV
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();         
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<CQData> ss = new List<CQData>();
            ss.Add(new CQData() {ID=1, Name="Q1", Time = DateTime.Now.AddHours(-3) , Test="Test_1"});
            ss.Add(new CQData() { ID = 2, Name = "Q2", Time = DateTime.Now.AddHours(-2), Test = "Test_2" });
            ss.Add(new CQData() { ID = 3, Name = "Q3", Time = DateTime.Now.AddHours(-1), Test = "Test_3" });
            string str = CQCSVConvert.SerializeObject(new CQData() { ID = 1, Name = "Q1", Time = DateTime.Now.AddHours(-3), Test = "Test_1" });
            //string str = CQCSVConvert.SerializeObject(ss);

            //File.WriteAllText("AAA.txt", str);
            //string data = File.ReadAllText("AAA.txt");
            //List<CQData> bb = CQCSVConvert.DeserializeObject<List<CQData>>(data);
        }
    }

    

    public class CQData
    {
        public CQData()
        {
            //this.ID = DateTime.Now.Millisecond;
            //this.Name = "AA";
            //this.Time = DateTime.Now;
        }
        [CQCSVProperty(Name = "編號", Column =5)]
        public int ID { set; get; }
        [CQCSVProperty(Name = "名稱", Column =2)]
        [CQCSVIgnore]
        public string Name { set; get; }
        [CQCSVProperty(Name = "Time", Column =0, Format ="yyyy/MM/dd HH:mm:ss")]
        public DateTime Time { set; get; }
        [CQCSVIgnore]
        public string Test { set; get; }

        public override string ToString()
        {
            return string.Format("ID:{0} Name:{1} Time:{2} Test:{3}"
                , this.ID
                , this.Name
                , this.Time.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                , this.Test);
        }
    }

}
