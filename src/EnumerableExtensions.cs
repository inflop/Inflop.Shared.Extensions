using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inflop.Shared.Extensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Checks if the collection is null or empty.
		/// </summary>
		/// <typeparam name="T">The type of the elements of <paramref name="source"/></typeparam>
		/// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>
		/// <returns>
		/// <c>true</c> if the <paramref name="source"/> sequence is null or empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsEmpty<T>(this IEnumerable<T> source)
			=> source.IsNull() || !source.Any();

		/// <summary>
		/// Checks if the collection is not null or not empty.
		/// </summary>
		/// <typeparam name="T">The type of the elements of <paramref name="source"/></typeparam>
		/// <param name="source">The <see cref="IEnumerable{T}"/> to check.</param>
		/// <returns>
		/// <c>true</c> if the <paramref name="source"/> sequence is not null or not empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNotEmpty<T>(this IEnumerable<T> source)
			=> !source.IsEmpty();


		/// <summary>
		/// Determines whether a sequence contains exactly one element.
		/// </summary>
		/// <typeparam name="T">The type of the elements of <paramref name="source"/></typeparam>
		/// <param name="source">The <see cref="IEnumerable{T}"/> to check for singularity.</param>
		/// <returns>
		/// <c>true</c> if the <paramref name="source"/> sequence contains exactly one element; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsSingle<T>(this IEnumerable<T> source)
		{
			if (source.IsNull())
				throw new ArgumentNullException(nameof(source));

			using var enumerator = source.GetEnumerator();
			return enumerator.MoveNext() && !enumerator.MoveNext();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">The type of the elements of <paramref name="source"/></typeparam>
		/// <param name="source"></param>
		/// <param name="action"></param>
		/// <returns>
		/// 
		/// </returns>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var i in source)
				action(i);

			return source;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="separator"></param>
		/// <param name="withHeaders"></param>
		/// <param name="columnNames"></param>
		/// <param name="dateTimeFormat"></param>
		/// <returns></returns>
		public static string ToCsv<T>(this IEnumerable<T> source, string separator, bool withHeaders = true, IEnumerable<string> columnNames = null, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
		{
			if (source.IsNull() || source.Count() <= 0)
				return string.Empty;

			var fieldsArray = typeof(T).GetFields();
			var propertiesArray = typeof(T).GetProperties();

			if (columnNames.IsNotNull() && columnNames.Count() > 0)
			{
				var propertiesList = new List<PropertyInfo>(propertiesArray);
				propertiesList.RemoveAll(item => !columnNames.Contains(item.Name));
				propertiesArray = propertiesList.ToArray();
			}

			StringBuilder csvBuilder = new StringBuilder();

			if (withHeaders)
				csvBuilder.AppendLine(string.Join(separator, fieldsArray.Select(f => f.Name).Union(propertiesArray.Select(p => p.Name.ToCsvValue(separator))).ToArray()));

			source.ToList<T>().ForEach(item =>
			{
				csvBuilder.AppendLine(string.Join(separator, propertiesArray.Select(p => p.GetValue(item, null).ToCsvValue(separator)).ToArray()));
			});

			return csvBuilder.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item"></param>
		/// <param name="separator"></param>
		/// <param name="dateTimeFormat"></param>
		/// <returns></returns>
		private static string ToCsvValue<T>(this T item, string separator, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
		{
			if (item.IsNull() || (item.IsNotNull() && item.ToString().IsDateTime() && item.ToDateTime().IsDefaultDateTime()))
				return "\"\"";

			string result = string.Format("\"{0}\"", item.ToString().Replace("\"", "\\\"")).Replace("\n", " ").Replace("\r", " ").Replace(separator, " ");

			if (item.ToString().IsNumeric())
				result = string.Format("={0}", result);

			if (item.ToString().IsDateTime())
				result = item.ToDateTime().ToString(dateTimeFormat);

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static DataTable ToDataTable<T>(this IEnumerable<T> items)
		{
			var tb = new DataTable(typeof(T).Name);

			PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo prop in props)
			{
				Type t = prop.PropertyType.GetCoreType();
				tb.Columns.Add(prop.Name, t);
			}

			foreach (T item in items)
			{
				var values = new object[props.Length];

				for (int i = 0; i < props.Length; i++)
				{
					values[i] = props[i].GetValue(item, null);
				}

				tb.Rows.Add(values);
			}

			return tb;
		}
	}
}
