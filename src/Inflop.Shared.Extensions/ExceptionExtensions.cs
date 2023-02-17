using System.Collections;
using System.Text;

namespace Inflop.Shared.Extensions;

/// <summary>
/// Extension methods for the <see cref="System.Exception"/> data type.
/// </summary>
public static class ExceptionExtensions
{
    private static int exceptionLevel = 0;

    /// <summary>
    /// Returns full information about the exception
    /// </summary>
    /// <param name="ex">Thrown an exception to be reported.</param>
    /// <param name="htmlFormatted">Replace newline to HTML tag. Default <c>false</c>. </param>
    /// <returns></returns>
    public static string FullInfo(this Exception ex, bool htmlFormatted = false)
    {
        StringBuilder sb = new StringBuilder();
        string boldFontTagOpen = htmlFormatted ? "<b>" : "";
        string boldFontTagClose = htmlFormatted ? "</b>" : "";
        string redFontTagOpen = htmlFormatted ? "<font color='red'>" : "";
        string redFontTagClose = htmlFormatted ? "</font>" : "";

        exceptionLevel++;
        string indent = new string('\t', exceptionLevel - 1);
        sb.Append($"{boldFontTagOpen}{indent}*** Exception level {exceptionLevel} *************************************************{boldFontTagClose}{Environment.NewLine}");
        sb.Append($"{boldFontTagOpen}{indent}ExceptionType:{boldFontTagClose} {ex.GetType().Name}{Environment.NewLine}");
        sb.Append($"{boldFontTagOpen}{indent}HelpLink:{boldFontTagClose} {ex.HelpLink}{Environment.NewLine}");
        sb.Append($"{boldFontTagOpen}{indent}Message: {redFontTagOpen}{ex.Message}{redFontTagClose}{boldFontTagClose}{Environment.NewLine}");
        sb.Append($"{boldFontTagOpen}{indent}Source:{boldFontTagClose} {ex.Source}{Environment.NewLine}");
        sb.Append($"{boldFontTagOpen}{indent}StackTrace:{boldFontTagClose} {ex.StackTrace}{Environment.NewLine}");
        sb.Append($"{boldFontTagOpen}{indent}TargetSite:{boldFontTagClose} {ex.TargetSite}{Environment.NewLine}");

        if (ex.Data.Count > 0)
        {
            sb.Append($"{boldFontTagOpen}{indent}Data:{boldFontTagClose}{Environment.NewLine}");
            foreach (DictionaryEntry de in ex.Data)
                sb.Append($"{indent}\t{de.Key} : {de.Value}");
        }

        Exception innerException = ex.InnerException;

        while (innerException.IsNotNull())
        {
            sb.Append(innerException.FullInfo());
            if (exceptionLevel > 1)
                innerException = innerException.InnerException;
            else
                innerException = null;
        }

        exceptionLevel--;

        string result = htmlFormatted ? sb.ToString().Replace(Environment.NewLine, "<br />") : sb.ToString();

        return result;
    }

    /// <summary>
    /// Returns result from <see cref="System.Exception.GetBaseException()"/>.
    /// </summary>
    /// <param name="ex">Thrown an exception to be reported.</param>
    /// <returns></returns>
    public static Exception BaseException(this Exception ex)
        => ex.GetBaseException();

    /// <summary>
    /// Returns exception message from <see cref="System.Exception.GetBaseException()"/>.
    /// </summary>
    /// <param name="ex">Thrown an exception to be reported.</param>
    /// <returns></returns>
    public static string BaseExceptionMessage(this Exception ex)
        => ex.BaseException().Message;
}
