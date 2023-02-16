using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Inflop.Shared.Extensions;

public static class DbParameterCollectionExtensions
{
    public static T Map<T>(this DbParameterCollection parameters, string prefix = "") where T : class, new()
    {
        T result = new T();
        PropertyInfo[] propertyInfos = typeof(T).GetProperties();

        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            foreach (DbParameter parameter in parameters)
            {
                if (parameter.Direction != ParameterDirection.InputOutput)
                    continue;

                if (parameter.ParameterName.StartsWith("@"))
                    parameter.ParameterName = parameter.ParameterName.TrimStart('@');

                if (propertyInfo.Name == $"{prefix}{parameter.ParameterName}")
                {
                    propertyInfo.SetValue(result, Convert.ChangeType(parameter.Value, propertyInfo.PropertyType), null);
                    break;
                }
            }
        }

        return result;
    }
}
