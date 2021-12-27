using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Inflop.Shared.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="System.Object"/> data type.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static bool Between<T>(this T self, T lower, T upper) where T : IComparable<T>
            => self.CompareTo(lower) >= 0 && self.CompareTo(upper) <= 0;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T value, params T[] list)
            => list.Contains(value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T value, params T[] list)
            => !value.In<T>(list);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefault<T>(this T value) where T : struct
            => value.Equals(default(T));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(this object value)
            => value == null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object value)
            => !value.IsNull();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Tuple<bool, DateTime> TryParseToDateTime(this object date)
            => date.IsNull()
                ? new Tuple<bool, DateTime>(false, DateTimeExtensions.DefaultDateTime)
                : date.ToString().TryParseToDateTime();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object date)
            => date.IsNull()
                ? DateTimeExtensions.DefaultDateTime
                : date.ToString().ToDateTime();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBoolean(this object value)
            => value.IsNull()
                ? false
                : value.ToString().IsBoolean();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object value)
            => value.IsNull()
                ? false
                : value.ToString().ToBoolean();

        public static bool IsNumeric(this object value)
        {
            if (value.IsNull())
                return false;

            return value.ToString().IsNumeric();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object value)
            => value.IsNull()
                ? 0
                : value.ToString().ToDecimal();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInteger(this object value)
        {
            return value.IsNull() ? 0 : value.ToString().ToInteger();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this object value)
        {
            return value.IsNull() ? 0 : value.ToString().ToLong();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T To<T>(this IConvertible value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToDefault<T>(this T value)
        {
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(this object obj, bool formatIndented = false)
        {
            string result = string.Empty;

            if (obj.IsNull()) return result;

            Newtonsoft.Json.Formatting formatting = formatIndented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

            try
            {
                result = JsonConvert.SerializeObject(obj, formatting);
            }
            catch(Exception ex)
            {
                result = ex.BaseExceptionMessage();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToXml<T>(this T value, XmlSerializerNamespaces namespaces = null, bool indent = false, Encoding encoding = null)
        {
            if (value.IsNull())
                return string.Empty;

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = indent;

            if (encoding.IsNull())
                encoding = Encoding.UTF8;

            xmlWriterSettings.Encoding = encoding;

            using (var stringWriter = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                xmlserializer.Serialize(writer, value, namespaces);
                return stringWriter.ToString();
            }
        }

        public static byte[] ToByteArray<T>(this T value)
        {
            if (value.IsNull())
                return null;

            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
        }

        public static T ToObject<T>(this byte[] value)
        {
            if (value.IsNull())
                return default(T);

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));
        }

        public static string ToHttpQueryStringParams(this object value)
        {
            Dictionary<string, string> @params = value.GetType().GetProperties().AsEnumerable().ToDictionary(k => k.Name, v => v.GetValue(value, null).ToString());
            return @params.ToHttpQueryStringParams();
        }

        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
    }
}
