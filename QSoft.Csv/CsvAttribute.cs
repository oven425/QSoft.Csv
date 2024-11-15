using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QSoft.Csv
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =false)]
    public class CsvIgnoreAttribute:Attribute
    {
    }


}
