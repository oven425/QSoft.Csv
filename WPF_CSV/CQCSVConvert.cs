using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QCSV
{
    public class CQCSVConvert
    {
        static CQCSVConvert()
        {
        }
        public static T DeserializeObject<T>(string data, string split = ",", string wrap = "\r\n") where T : new()
        {
            T aa = new T();
            Type type = typeof(T);
            Type[] tts = type.GetGenericArguments();
            if(tts.Length <= 0)
            {
                return aa;
            }
            PropertyInfo[] pps1 = tts[0].GetProperties();
            List<PropertyInfo> pps = new List<PropertyInfo>();
            for(int i=0; i<pps1.Length; i++)
            {
                object[] ccu = pps1[i].GetCustomAttributes(typeof(CQCSVIgnore), true);
                if (ccu.Length == 0)
                {
                    pps.Add(pps1[i]);
                }
            }
            if(pps.Count <= 0)
            {
                return aa;
            }
            Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
            for(int i=0; i<pps.Count; i++)
            {
                object[] ccu = pps[i].GetCustomAttributes(typeof(CQCSVProperty), true);
                if(ccu.Length > 0)
                {
                    dic.Add(((CQCSVProperty)ccu[0]).Name, pps[i]);
                }
                else
                {
                    dic.Add(pps[i].Name, pps[i]);
                }
            }

            MethodInfo method = type.GetMethod("Add");


            string[] sl_1 = data.Split(new string[] { wrap }, StringSplitOptions.RemoveEmptyEntries);
            List<PropertyInfo> cols = new List<PropertyInfo>();
            for (int i = 0; i < sl_1.Length; i++)
            {
                if (i == 0)
                {
                    string[] sl_2 = sl_1[i].Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
                    for(int j=0; j<sl_2.Length; j++)
                    {
                        if(dic.ContainsKey(sl_2[j]) == true)
                        {
                            cols.Add(dic[sl_2[j]]);
                        }
                        else
                        {
                            cols.Add(null);
                        }
                    }
                    
                }
                else
                {
                    string[] sl_2 = sl_1[i].Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
                    object instance = Activator.CreateInstance(tts[0]);

                    for (int j = 0; j < sl_2.Length; j++)
                    {
                        if(cols[j] != null)
                        {
                            ToValue(cols[j], sl_2[j], instance);
                        }
                        
                    }
                    method.Invoke(aa, new object[] { instance });
                }
            }

            return aa;
        }

        static void ToValue(PropertyInfo property, string value, object instance)
        {
            if (property.PropertyType.FullName == typeof(DateTime).FullName)
            {
                property.SetValue(instance, Convert.ToDateTime(value), null);
            }
            else if (property.PropertyType.FullName == typeof(int).FullName)
            {
                property.SetValue(instance, Convert.ToInt32(value), null);
            }
            else if (property.PropertyType.FullName == typeof(uint).FullName)
            {
                property.SetValue(instance, Convert.ToUInt32(value), null);
            }
            else if (property.PropertyType.FullName == typeof(short).FullName)
            {
                property.SetValue(instance, Convert.ToInt16(value), null);
            }
            else if (property.PropertyType.FullName == typeof(ushort).FullName)
            {
                property.SetValue(instance, Convert.ToUInt16(value), null);
            }
            else if (property.PropertyType.FullName == typeof(long).FullName)
            {
                property.SetValue(instance, Convert.ToInt64(value), null);
            }
            else if (property.PropertyType.FullName == typeof(ulong).FullName)
            {
                property.SetValue(instance, Convert.ToUInt64(value), null);
            }
            else if (property.PropertyType.FullName == typeof(byte).FullName)
            {
                property.SetValue(instance, Convert.ToByte(value), null);
            }
            else if (property.PropertyType.FullName == typeof(sbyte).FullName)
            {
                property.SetValue(instance, Convert.ToSByte(value), null);
            }
            else if (property.PropertyType.FullName == typeof(string).FullName)
            {
                property.SetValue(instance, value, null);
            }
        }

        public static string SerializeObject(object data, string split = ",", string wrap = "\r\n")
        {
            StringBuilder strb = new StringBuilder();

            Type type = data.GetType();
            MethodInfo method_count = type.GetMethod("get_Count");
            int count = (int)method_count.Invoke(data, null);

            List<PropertyInfo> pps = new List<PropertyInfo>();
            MethodInfo method = type.GetMethod("get_Item");
            for (int i = 0; i < count; i++)
            {
                object vv = method.Invoke(data, new object[] { i });
                if (i==0)
                {
                    Type type1 = vv.GetType();
                    PropertyInfo[] pps1 = type1.GetProperties();
                    List<PropertyInfo> pps2 = new List<PropertyInfo>();
                    for (int j = 0; j < pps1.Length; j++)
                    {
                        object[] ccu = pps1[j].GetCustomAttributes(typeof(CQCSVIgnore), true);
                        if (ccu.Length == 0)
                        {
                            pps2.Add(pps1[j]);
                        }
                    }

                    SortedDictionary<int, List<PropertyInfo>> pps3 = new SortedDictionary<int, List<PropertyInfo>>();
                    for (int j = 0; j < pps2.Count; j++)
                    {
                        object[] ccu = pps1[j].GetCustomAttributes(typeof(CQCSVProperty), true);
                        if (ccu.Length > 0)
                        {
                            CQCSVProperty pp = ccu[0] as CQCSVProperty;
                            if (pp != null)
                            {
                                if (pps3.ContainsKey(pp.Column) == true)
                                {
                                    pps3[pp.Column].Add(pps2[j]);
                                }
                                else
                                {
                                    pps3.Add(pp.Column, new List<PropertyInfo>() { pps2[j] });
                                }

                            }

                        }
                    }
                    for(int j=0; j<pps3.Count; j++)
                    {
                        pps.AddRange(pps3.ElementAt(j).Value);
                    }
                }
               
                
                


                StringBuilder strb1 = null;
                if (i == 0)
                {
                    strb1 = new StringBuilder();
                }
                StringBuilder strb2 = new StringBuilder();

                foreach (PropertyInfo pi in pps)
                {
                    if (i == 0)
                    {
                        object[] ccu = pi.GetCustomAttributes(typeof(CQCSVProperty), true);
                        string col_name = pi.Name;
                        if (ccu.Length > 0)
                        {
                            if (string.IsNullOrEmpty(((CQCSVProperty)ccu[0]).Name) == false)
                            {
                                col_name = ((CQCSVProperty)ccu[0]).Name;
                            }
                        }
                        if (strb1.Length > 0)
                        {
                            strb1.Append(split);
                            strb1.Append(col_name);
                        }
                        else
                        {
                            strb1.Append(col_name);
                        }
                    }

                    object oob = pi.GetValue(vv, null);
                    if (strb2.Length > 0)
                    {
                        strb2.Append(split);
                        if(oob != null)
                        {
                            strb2.Append(oob.ToString());
                        }
                        
                    }
                    else
                    {
                        strb2.Append(oob.ToString());
                    }

                }
                if (strb1 != null)
                {
                    strb.Append(strb1.ToString());
                    strb.Append(wrap);
                }
                strb.Append(strb2.ToString());
                strb.Append(wrap);
            }

            return strb.ToString();
        }
    }

    public class CQCSVProperty : Attribute
    {
        public string Name { set; get; }
        public string Format { set; get; }
        public int Column { set; get; }
    }

    public class CQCSVIgnore:Attribute
    {

    }
}
