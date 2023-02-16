using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Inflop.Shared.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCoreType(this Type type)
        {
            if (type != null && type.IsNullable())
            {
                if (!type.IsValueType)
                {
                    return type;
                }
                else
                {
                    return Nullable.GetUnderlyingType(type);
                }
            }
            else
            {
                return type;
            }
        }
        public static string ToSha256Hash(this byte[] array)
        {
            if (array is null || array.Length == 0)
                throw new ArgumentException("Input array cannot be empty!");
            
            byte[] hashValue = null;
            HashAlgorithm hash = SHA256.Create();
            hashValue = hash.ComputeHash(array);

            return Convert.ToBase64String(hashValue);            
        }
    }
}
