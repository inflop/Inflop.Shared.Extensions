using System.Data;
using System.Dynamic;
using System.Reflection;

namespace Inflop.Shared.Extensions;

public static class DataReaderExtensions
{
    /// <summary>
    /// Tworzy i zwraca obiekt typu {T}.
    /// Mapuje wiersz obiektu <see cref="System.Data.IDataReader"/> na klasę {T}.
    /// Jeśli nazwa wiersza jest identyczna z nazwą publicznej właściwości klasy
    /// to wartość z teg owiersza przepisuje do wartości tej właściwości obiektu.
    /// </summary>
    /// <typeparam name="T">Typ obiektu na jaki ma być mapowany odczytany rekord.</typeparam>
    /// <param name="record">Wiersz obiektu <see cref="System.Data.IDataReader"/>, z któego odczytywane będą wartości.</param>
    /// <returns></returns>
    public static T Map<T>(this IDataRecord record) where T : class, new()
    {
        T result = new T();

        PropertyInfo[] propertyInfos = typeof(T).GetProperties();

        if ((propertyInfos == null || !propertyInfos.Any()) && result is ExpandoObject)
        {
            var properties = new Dictionary<string, object>();
            var dynamicObject = new ExpandoObject() as IDictionary<string, Object>;
            for (int i = 0; i < record.FieldCount; i++)
            {
                properties.Add(record.GetName(i), record.GetFieldType(i));
                dynamicObject.Add(record.GetName(i), Convert.ChangeType(record.GetValue(i), record.GetFieldType(i)));
            }
            result = dynamicObject as T;
        }
        else
            for (int i = 0; i < record.FieldCount; i++)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.Name == record.GetName(i))
                    {
                        object value = record.GetValue(i);

                        if (!(value is DBNull))
                            propertyInfo.SetValue(result, Convert.ChangeType(value, record.GetFieldType(i)), null);

                        break;
                    }
                }
            }

        return result;
    }
}
