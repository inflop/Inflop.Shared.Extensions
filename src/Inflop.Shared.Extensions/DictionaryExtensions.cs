using System.Reflection;

namespace Inflop.Shared.Extensions;

public static class DictionaryExtensions
{
    public static T Map<T>(this IDictionary<string, object> dict)
        where T : class, new()
    {
        T result = new T();
        PropertyInfo[] propertyInfos = typeof(T).GetProperties();

        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            foreach (KeyValuePair<string, object> kv in dict)
            {
                if (kv.Key == propertyInfo.Name)
                {
                    propertyInfo.SetValue(result, Convert.ChangeType(kv.Value, propertyInfo.GetType()), null);
                    break;
                }
            }
        }

        return result;
    }

    public static string ToHttpQueryStringParams(this IDictionary<string, string> @params)
        => string.Join("&", @params.Select(kv => $"{kv.Key}={kv.Value}"));
}
