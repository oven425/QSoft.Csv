using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QCSV
{
    public class CQCSVConvert
    {
        public static T DeserializeObject<T>(string data) where T:new()
        {
            T aa = new T();
            string[] sl_1 = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);



            return aa;
        }

        public static string SerializeObject(object data, string split = ",")
        {
            StringBuilder strb = new StringBuilder();

            Type type = data.GetType();
            MethodInfo method_count = type.GetMethod("get_Count");
            int count = (int)method_count.Invoke(data, null);

            MethodInfo method = type.GetMethod("get_Item");
            for (int i = 0; i < count; i++)
            {
                
                object vv = method.Invoke(data, new object[] { i });
                Type type1 = vv.GetType();
                PropertyInfo[] pps = type1.GetProperties();
                StringBuilder strb1 = null;
                if (i == 0)
                {
                    strb1 = new StringBuilder();
                }
                StringBuilder strb2 = new StringBuilder();
                
                foreach (PropertyInfo pi in pps)
                {
                    if(i==0)
                    {
                        object[] ccu = pi.GetCustomAttributes(typeof(CQCSVProperty), true);
                        string col_name = pi.Name;
                        if(ccu.Length >0)
                        {
                            if(string.IsNullOrEmpty(((CQCSVProperty)ccu[0]).Name) == false)
                            {
                                col_name = ((CQCSVProperty)ccu[0]).Name;
                            }
                        }
                        if (strb1.Length > 0)
                        {
                            strb1.Append(",");
                            strb1.Append(col_name);
                        }
                        else
                        {
                            strb1.Append(col_name);
                        }
                    }
                    

                    object oob = pi.GetValue(vv, null);
                    if(strb2.Length >0)
                    {
                        strb2.Append(",");
                        strb2.Append(oob.ToString());
                    }
                    else
                    {
                        strb2.Append(oob.ToString());
                    }
                    
                }
                if (strb1 != null)
                {
                    strb.AppendLine(strb1.ToString());
                }
                strb.AppendLine(strb2.ToString());
            }

            return strb.ToString();
        }
    }

    public class CQCSVProperty : Attribute
    {
        public string Name { set; get; }
        public string Format { set; get; }
    }
}
