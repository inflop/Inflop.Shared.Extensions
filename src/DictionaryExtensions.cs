using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Inflop.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static T Map<T>(this IDictionary<string, object> dict)
            where T: class, new()
        {
			T result = new T();
			PropertyInfo[] propertyInfos = typeof(T).GetProperties();

			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
                foreach (KeyValuePair<string, object> kv in dict)
                {
                    if(kv.Key == propertyInfo.Name)
                    {
						propertyInfo.SetValue(result, Convert.ChangeType(kv.Value, propertyInfo.GetType()), null);
                        break;
                    }
                }
			}

			return result;
		}

        public static string ToHttpQueryStringParams(this IDictionary<string, string> @params)
        {
            string result = string.Format("{0}",
					string.Join("&",
						@params.Select(kvp =>
							string.Format("{0}={1}", kvp.Key, kvp.Value))));

            return result;
        }
    }
}