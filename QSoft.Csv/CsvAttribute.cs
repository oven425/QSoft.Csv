using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSoft.Csv
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =false)]
    public class CsvIgnoreAttribute:Attribute
    {
    }


    public class CsvWriter
    {

        //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/raw-string
        public void Write()
        {
            var singleLine = """This is a "raw string literal". It can contain characters like \, ' and ".""";
            var xml = """
        <element attr="content">
            <body>
            </body>
        </element>
        """;
        }
    }
    
}
