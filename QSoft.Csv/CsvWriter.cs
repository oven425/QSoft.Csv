using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QSoft.Csv
{
    public class CsvWriter<T>:IDisposable
    {
        readonly List<PropertyInfo> m_PPs = [];
        readonly StreamWriter? m_SW;
        public CsvWriter(Stream stream)
        {
            m_SW = new StreamWriter(stream);
            
            this.m_PPs.AddRange(typeof(T).GetProperties()
                .Where(x => x.CanRead));
        }

        public void Dispose()
        {
            m_SW?.Dispose();
        }

        public void Write(T data)
        {
            var values = m_PPs.Select(x => x.GetValue(data)?.ToString() ?? "")
                .Aggregate((x,y)=>$"{x}{y}");
            this.m_SW?.WriteLine(values);
            
        }
    }

    
}
