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
            //List<CQData> ss = new List<CQData>();
            //ss.Add(new CQData());
            //ss.Add(new CQData());
            //ss.Add(new CQData());
            //string str = CQCSVConvert.SerializeObject(ss);
            //File.WriteAllText("AAA.txt", str);
            string data = File.ReadAllText("AAA.txt");
            CQCSVConvert.DeserializeObject<List<CQData>>(data);
        }
    }

    

    public class CQData
    {
        public CQData()
        {
            this.ID = DateTime.Now.Millisecond;
            this.Name = "AA";
            this.Time = DateTime.Now;
        }
        //[CQCSVProperty(Name="編號")]
        public int ID { set; get; }
        //[CQCSVProperty(Name = "名稱")]
        public string Name { set; get; }
        public DateTime Time { set; get; }
    }

}
