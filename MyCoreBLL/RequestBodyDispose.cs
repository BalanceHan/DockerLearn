using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace MyCoreBLL
{
    public class RequestBodyDispose
    {
        public static string Operation(string result)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(result);
            var arr = new Dictionary<object, object>();
            var dict = query.AllKeys.ToDictionary(k => k, k => query[k]);
            string flag = string.Empty;
            List<object> list = new List<object>();
            foreach (var item in dict)
            {
                if (item.Key.Contains("[") && item.Key.Contains("]"))
                {
                    string sub = item.Key.Substring(0, item.Key.IndexOf("["));
                    if (sub == flag)
                    {
                        list.Add(item.Value);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(flag))
                        {
                            arr.Add(flag, list);
                            flag = sub;
                            list.Clear();
                            list.Add(item.Value);
                        }
                        else
                        {
                            flag = sub;
                            list.Add(item.Value);
                        }
                    }
                }
                else
                {
                    arr.Add(item.Key, item.Value);
                }
            }
            arr.Add(flag, list);
            return JsonConvert.SerializeObject(arr);
        }
    }
}
