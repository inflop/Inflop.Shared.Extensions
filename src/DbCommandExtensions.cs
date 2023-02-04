using System.Data;
using Microsoft.Data.SqlClient;

namespace Inflop.Shared.Extensions
{
    public static class DbCommandExtensions
    {
        public static void AddParameter<T>(this IDbCommand command, string name, T value)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (name == null)
                throw new ArgumentNullException("name");

            var parameter = command.CreateParameter();
            parameter.ParameterName = name;

            if (value.IsNull())
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value;

            command.Parameters.Add(parameter);
        }

        public static void AddParameter(this IDbCommand command, string name, object value)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (name == null)
                throw new ArgumentNullException("name");

            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;

            command.Parameters.Add(parameter);
        }

        public static IEnumerable<T> ToList<T>(this IDbCommand command) where T : class, new()
        {
            List<T> result = new List<T>();

            using (var dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    result.Add(dr.Map<T>());
                }
            }

            return result;
        }

        public static string ToExecuteStatement(this IDbCommand command)
        {
            var commandParams = command.Parameters.Cast<SqlParameter>().Select(p => new { Name = p.ParameterName, Value = p.Value });
            return $"EXEC {command.CommandText} {string.Join(", ", commandParams.Select(p => string.Format("{0}={1}", p.Name, p.Value.IsNumeric() ? p.Value : $"'{p.Value}'")))}";
        }
    }
}
