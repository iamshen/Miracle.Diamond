using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace netCore.Utitly
{
    public static class Extension
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static void CheckNotNull(this object obj, string name)
        {
            if (obj == null)
                throw new ArgumentException($"参数:{name}为空!");
        }
    }
}
