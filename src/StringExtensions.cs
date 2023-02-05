using System.Globalization;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inflop.Shared.Extensions;

/// <summary>
/// Extension methods for the <see cref="System.String"/> data type.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts the string to its <see cref="System.DateTime"/> equivalent.
    /// A return value indicates whether the conversion succeeded.
    /// If conversion succeeded, returns <see langword="true"/> and parsed <see cref="System.DateTime"/>.
    /// If failed, returns <see langword="false"/> and <see cref="DateTimeExtensions.DefaultDateTime">DateTimeExtensions.DefaultDateTime</see>.
    /// </summary>
    /// <param name="dateString">The input string to parse.</param>
    /// <param name="format">The date format for input string to parse.</param>
    /// <returns>
    /// Returns <see langword="true"/> and parsed <see cref="System.DateTime"/> if succeeded.
    /// If failed, returns <see langword="false"/> and <see cref="DateTimeExtensions.DefaultDateTime">DateTimeExtensions.DefaultDateTime</see>.
    /// </returns>
    /// <remarks>
    /// If the parameter <paramref name="format"/> has default empty value, conversion is based on <see cref="Inflop.Shared.Extensions.DateTimeExtensions.DATE_FORMATS"/>.
    /// Otherwise parsing is based on <paramref name="format"/> value.
    /// </remarks>
    public static (bool Parsed, DateTime Value) TryParseToDateTime(this string dateString, string format = "")
    {
        dateString = dateString ?? DateTimeExtensions.DefaultDateTime.Date.ToString();
        DateTime dateTime = DateTimeExtensions.DefaultDateTime;
        bool parseResult = false;

        if (format.IsNotEmpty())
        {
            parseResult = DateTime.TryParseExact(dateString.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
        }
        else
        {
            foreach (string dateFormat in DateTimeExtensions.DATE_FORMATS)
            {
                parseResult = DateTime.TryParseExact(dateString.Trim(), dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (parseResult) break;
            }
        }

        if (dateTime == DateTime.MinValue)
            dateTime = DateTimeExtensions.DefaultDateTime;

        return (parseResult, dateTime);
    }

    /// <summary>
    /// Determines whether the specified string can convert to <see cref="System.DateTime"/>.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <param name="format">The date format for input string to parse.</param>
    /// <returns>Returns <see langword="true"/> if input string is <see cref="System.DateTime"/> equivalent, otherwise returns <see langword="false"/>.</returns>
    public static bool IsDateTime(this string value, string format = "")
        => value.TryParseToDateTime(format).Parsed;

    /// <summary>
    /// Converts input string to <see cref="System.DateTime"/>.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="format">The date format for input string to parse.</param>
    /// <returns>If convertion succeed, returns parsed value. If failed, returns <see cref="DateTimeExtensions.DefaultDateTime">DateTimeExtensions.DefaultDateTime</see>.</returns>
    /// <remarks>
    /// Conversion is based on method <see cref="Inflop.Shared.Extensions.StringExtensions.TryParseToDateTime(string, string)"/>.
    /// </remarks>
    public static DateTime ToDateTime(this string value, string format = "")
        => value.TryParseToDateTime(format).Value;

    /// <summary>
    /// Determines whether the specified string can convert to <see cref="System.Boolean"/>.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <returns>Returns <c>true</c> if the conversion of input string was successfull. Otherwise, it returns <c>false</c>.</returns>
    /// <remarks>
    /// Based on <see cref="Inflop.Shared.Extensions.BooleanExtensions.BOOLEAN_MAPPING"/>
    /// </remarks>
    public static bool IsBoolean(this string value)
    {
        var itemValue = BooleanExtensions.BOOLEAN_MAPPING.Where(v => v.Key == value.RemoveWhiteSpaces().ToUpperTrim()).FirstOrDefault();
        return !itemValue.IsDefault();
    }

    /// <summary>
    /// Converts input string to <see cref="System.Boolean"/>.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <returns>Returns <c>true</c> if the conversion of input string was successfull. Otherwise, it returns <c>false</c>.</returns>
    /// <remarks>
    /// Convertion is based on <see cref="Inflop.Shared.Extensions.BooleanExtensions.BOOLEAN_MAPPING"/>.
    /// </remarks>
    public static bool ToBoolean(this string value)
    {
        bool result = false;
        var itemValue = BooleanExtensions.BOOLEAN_MAPPING.Where(v => v.Key == value.RemoveWhiteSpaces().ToUpperTrim()).FirstOrDefault();
        if (!itemValue.IsDefault())
            result = itemValue.Value;

        return result;
    }

    /// <summary>
    /// Determines whether the specified string can convert to <see cref="System.Decimal"/>.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <returns>Returns <c>true</c> if the conversion of input string was successfull. Otherwise, it returns <c>false</c>.</returns>
    public static bool IsDecimal(this string value)
        => decimal.TryParse(value.Replace(".", ","), NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal result);

    /// <summary>
    /// Converts input string to <see cref="System.Decimal"/>.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <returns>If convertion succeed , returns parsed value. If failed, returns 0.</returns>
    public static decimal ToDecimal(this string value)
    {
        decimal.TryParse(value.Replace(".", ","), NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal result);
        return result;
    }

    /// <summary>
    /// Determines whether the specified string can convert to <see cref="System.Int32"/>.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <returns>Returns <c>true</c> if the conversion of input string was successfull. Otherwise, it returns <c>false</c>.</returns>
    public static bool IsInteger(this string value)
        => int.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out int result);

    /// <summary>
    /// Converts input string to <see cref="System.Int32"/>.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <returns>If convertion succeed , returns parsed value. If failed, returns 0.</returns>
    public static int ToInteger(this string value)
    {
        int.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out int result);
        return result;
    }

    /// <summary>
    /// Determines whether the specified string can convert to <see cref="System.Int64"/>.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <returns>Returns <c>true</c> if the conversion of input string was successfull. Otherwise, it returns <c>false</c>.</returns>
    public static bool IsLong(this string value)
        => long.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out long result);

    /// <summary>
    /// Converts input string to <see cref="System.Int64"/>.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <returns>If convertion succeed , returns parsed value. If failed, returns 0.</returns>
    public static long ToLong(this string value)
    {
        long.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out var result);
        return result;
    }

    /// <summary>
    /// Determines whether the specified string is null, empty or whitespace.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <returns></returns>
    public static bool IsEmpty(this string value)
        => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Determines whether the specified string is not null, empty or whitespace.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <returns></returns>
    public static bool IsNotEmpty(this string value)
        => !value.IsEmpty();

    /// <summary>
    /// Returns a copy of this string converted to uppercase in which all leading and trailing occurrences
    /// of a set of specified characters from the current String object are removed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToUpperTrim(this string value)
        => value.Trim().ToUpper();

    /// <summary>
    /// Returns a copy of this string converted to lowercase in which all leading and trailing occurrences
    /// of a set of specified characters from the current String object are removed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToLowerTrim(this string value)
        => value.Trim().ToLower();

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string RemoveWhiteSpaces(this string value)
        => Regex.Replace(value, @"\s|\t|\n|\r", string.Empty);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid ToGuid(this string value)
    {
        Guid guid = Guid.Empty;
        Guid.TryParse(value, out guid);
        return guid;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToSha256Hash(this string value)
    {
        var hashValue = SHA256.Create().ComputeHash(Encoding.Default.GetBytes(value));
        return Convert.ToBase64String(hashValue);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name = "encoding">
    /// The encoding to be used to convert.
    /// If <c>null</c> then use <see cref="Encoding.Default">Encoding.Default</see>.
    /// </param>
    /// <returns></returns>
    public static string ToSha256Hash(this string value, Encoding encoding)
    {
        var hashValue = SHA256.Create().ComputeHash((encoding ?? Encoding.Default).GetBytes(value));
        return Convert.ToBase64String(hashValue);
    }

    public static string ToMd5Hash(this string value)
    {
        var hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(value));

        var sb = new StringBuilder();
        foreach (var t in hash)
        {
            sb.Append(t.ToString("x2"));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Converts the string to a byte-array using the supplied encoding
    /// </summary>
    /// <param name="value">
    /// The input string.
    /// </param>
    /// <param name = "encoding">
    /// The encoding to be used to convert.
    /// If <c>null</c> then use <see cref="Encoding.Default">Encoding.Default</see>.
    /// </param>
    /// <returns>The created byte array</returns>
    public static byte[] ToBytes(this string value, Encoding encoding)
        => (encoding ?? Encoding.Default).GetBytes(value);

    /// <summary>
    /// Converts the string to a byte-array using the <see cref="Encoding.Default">Encoding.Default</see>
    /// </summary>
    /// <param name="value">The input string.</param>
    /// <returns>The created byte array</returns>
    public static byte[] ToBytes(this string value)
        => Encoding.Default.GetBytes(value);

    /// <summary>
    /// Converts the string to MemoryStream using the <see cref="Encoding.Default">Encoding.Default</see>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static MemoryStream ToMemoryStream(this string value, Encoding encoding)
    {
        var bytes = value.ToBytes(encoding);
        return new MemoryStream(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// Checks the input string is a is valid email address.
    /// </summary>
    /// <param name="value">The input string to check.</param>
    /// <returns>Returns <see langword="true"/> if valid, otherwise returns <see langword="false"/>.</returns>
    public static bool IsValidEmail(this string value)
    {
        if (value.IsEmpty())
            return false;

        try
        {
            var addr = new MailAddress(value);
            return addr.Address == value;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks the input string is a is valid NIP number.
    /// </summary>
    /// <param name="value">The input string to check.</param>
    /// <returns>Returns <see langword="true"/> if valid, otherwise returns <see langword="false"/>.</returns>
    public static bool IsValidNip(this string value)
    {
        if (value.IsEmpty() || (value.ToLower().StartsWith("pl") == false && new Regex(@"^\d+$").IsMatch(value)))
            return true;

        value = Regex.Replace(value, @"[^0-9.]", "");

        int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
        bool result = false;
        if (value.Length == 10)
        {
            int controlSum = CalculateControlSum(value, weights);
            int controlNum = controlSum % 11;
            if (controlNum == 10)
            {
                controlNum = 0;
            }
            int lastDigit = int.Parse(value[value.Length - 1].ToString());
            result = controlNum == lastDigit;
        }
        return result;
    }

    /// <summary>
    /// Checks the input string is a is valid REGON address.
    /// </summary>
    /// <param name="value">The input string to check.</param>
    /// <returns>Returns <see langword="true"/> if valid, otherwise returns <see langword="false"/>.</returns>
    public static bool IsValidRegon(this string value)
    {
        if (value.IsEmpty())
            return true;
        int controlSum;
        if (value.Length == 7 || value.Length == 9)
        {
            int[] weights = { 8, 9, 2, 3, 4, 5, 6, 7 };
            int offset = 9 - value.Length;
            controlSum = CalculateControlSum(value, weights, offset);
        }
        else if (value.Length == 14)
        {
            int[] weights = { 2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8 };
            controlSum = CalculateControlSum(value, weights);
        }
        else
        {
            return false;
        }

        int controlNum = controlSum % 11;
        if (controlNum == 10)
        {
            controlNum = 0;
        }
        int lastDigit = int.Parse(value[value.Length - 1].ToString());

        return controlNum == lastDigit;
    }

    /// <summary>
    /// Checks the input string is a is valid PESEL address.
    /// </summary>
    /// <param name="value">The input string to check.</param>
    /// <returns>Returns <see langword="true"/> if valid, otherwise returns <see langword="false"/>.</returns>
    public static bool IsValidPesel(this string value)
    {
        int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        bool result = false;
        if (value.Length == 11)
        {
            int controlSum = CalculateControlSum(value, weights);
            int controlNum = controlSum % 10;
            controlNum = 10 - controlNum;
            if (controlNum == 10)
            {
                controlNum = 0;
            }
            int lastDigit = int.Parse(value[value.Length - 1].ToString());
            result = controlNum == lastDigit;
        }
        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <param name="weights"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    private static int CalculateControlSum(string input, int[] weights, int offset = 0)
    {
        int controlSum = 0;
        for (int i = 0; i < input.Length - 1; i++)
        {
            controlSum += weights[i + offset] * int.Parse(input[i].ToString());
        }
        return controlSum;
    }

    /// <summary>
    /// Converts <see cref="System.String"/> into an <see cref="System.Enum"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="System.Enum"/>.</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="ignorecase">Ignore case or not.</param>
    /// <returns></returns>
    public static T ToEnum<T>(this string value, bool ignorecase = true)
    {
        if (value.IsNull())
            return default(T);

        value = value.Trim();

        if (value.Length == 0)
            return default(T);

        Type t = typeof(T);

        if (!t.IsEnum)
            return default(T);

        try
        {
            return (T)Enum.Parse(t, value, ignorecase);
        }
        catch
        {
            return default(T);
        }
    }

    /// <summary>
    /// Replace all multiple whitespaces with one space.
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <returns></returns>
    public static string Minify(this string value)
    {
        value = Regex.Replace(value, @"\s+", " ");
        return value;
    }

    /// <summary>
    /// Check specified path is directory or file and returns <see langword="true"/> or <see langword="false"/>.
    /// Returns <see langword="null" /> if path does not exists.
    /// </summary>
    /// <param name="path">Path to check.</param>
    /// <returns></returns>
    public static bool? IsDirectory(this string path)
    {
        bool? result = null;

        if (Directory.Exists(path) || File.Exists(path))
        {
            var fileAttr = File.GetAttributes(path);
            result = fileAttr.HasFlag(FileAttributes.Directory);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNumeric(this string value)
    {
        bool isNum;
        double retNum;

        isNum = double.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    // public static string ToDecryptedString(this string value)
    // {
    //     string result = value;

    //     try
    //     {
    //         result = CryptorEngine.Decrypt(value, true);
    //     }
    //     catch
    //     {
    //         result = value;
    //     }

    //     return result;
    // }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    // public static string ToEncryptedString(this string value)
    // {
    //     string result = value;

    //     // Zabezpiecznie przed podwójnym zaszyfrowaniem.
    //     // Sprawdzamy czy tekst można odszyfrować, jeśli tak tzn, że już jest zaszyfrowany
    //     // i nie szyfrujemy go ponownie ale zwracamy tą samą wartość jaka została przekazana do metody.
    //     string decryptedValue = value.ToDecryptedString();
    //     // jeśli wartości się różnią tzn. że pomyślnie odszyfrowano wartość,
    //     // a tzn. że wartość wejściowa była już zaszyfrowana. Nie szyfrujemy jej ponownie i zwracamy.
    //     bool isAlreadyEncrypted = (decryptedValue != value);
    //     if (isAlreadyEncrypted)
    //         return value;

    //     try
    //     {
    //         result = CryptorEngine.Encrypt(value, true);
    //     }
    //     catch
    //     {
    //         result = value;
    //     }

    //     return result;
    // }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="charCount"></param>
    /// <returns></returns>
    public static string TruncateTo(this string value, int charCount)
    {
        if (value.IsEmpty())
            return string.Empty;

        value = value.Substring(0, (value.Length > charCount ? charCount : value.Length));
        return value;
    }

    /// <summary>
    /// Deserializuje obiekt JSON zapisany jako łańcuch znaków na obiekt wybranego typu.
    /// Jeśli podczas próby deserializacji wystąpi błąd, zwrócona zostanie wartość null.
    /// (wykorzystuje bibliotekę Newtonsoft.Json)
    /// </summary>
    /// <typeparam name="T">
    /// Typ danych na jaki ma zostać zdeserializowany wejściowy łańcuch znakowy.
    /// </typeparam>
    public static T DeserializeJsonTo<T>(this string value) where T : class
    {
        try
        {
            return JsonConvert.DeserializeObject(value, typeof(T)) as T;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Parsuje obiekt JSON zapisany jako łańcuch znaków a następnie deserializuje go na obiekt wybranego typu.
    /// Jeśli podczas próby parsowania wystąpi błąd, zwrócona zostanie wartość null.
    /// (wykorzystuje bibliotekę Newtonsoft.Json)
    /// </summary>
    /// <typeparam name="T">
    /// Typ danych na jaki ma zostać sparsowany wejściowy łańcuch znakowy.
    /// </typeparam>
    public static T ParseJsonTo<T>(this string value) where T : class
    {
        try
        {
            return JObject.Parse(value).ToString().DeserializeJsonTo<T>();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Wypisuje podany ciąg znaków n-ilość razy.
    /// </summary>
    /// <param name="value">Ciąg znaków do powtórzenia.</param>
    /// <param name="count">Ilość powtórzeń</param>
    /// <returns></returns>
    public static string Repeat(this string value, int count) => new StringBuilder(value.Length * count).Insert(0, value, count).ToString();

    /// <summary>
    /// Konwertuje ciąg znaków zapisany jako CamelCase
    /// do ciągu znaków przedzielonych podekreśleniem camel_case
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FromCamelCaseToUndescored(this string value, bool toLower = true)
    {
        var result = string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));

        if (toLower)
            result = result.ToLower();

        return result;
    }
}
