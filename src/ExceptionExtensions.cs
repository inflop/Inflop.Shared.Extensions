using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inflop.Shared.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="System.Exception"/> data type.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        private static int exceptionLevel = 0;

        /// <summary>
        /// Logs full information about the exception.
        /// </summary>
        /// <param name="ex">Thrown an exception to be logged.</param>
        // public static void Log(this Exception ex, string additionalMessage = "")
        // {
        //     string message = ex.FullInfo();

        //     if (additionalMessage.IsNotEmpty())
        //     {
        //         additionalMessage = $"AdditionalMessage: {additionalMessage}";
		// 		message = $"{additionalMessage}{Environment.NewLine}{message}";
        //     }

        //     NLogger.ErrorException(message, ex);
        // }

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

            ExceptionExtensions.exceptionLevel++;
            string indent = new string('\t', ExceptionExtensions.exceptionLevel - 1);
            sb.AppendFormat("{3}{0}*** Exception level {1} *************************************************{4}{2}", indent, ExceptionExtensions.exceptionLevel, Environment.NewLine, boldFontTagOpen, boldFontTagClose);
            sb.AppendFormat("{3}{0}ExceptionType:{4} {1}{2}", indent, ex.GetType().Name, Environment.NewLine, boldFontTagOpen, boldFontTagClose);
            sb.AppendFormat("{3}{0}HelpLink:{4} {1}{2}", indent, ex.HelpLink, Environment.NewLine, boldFontTagOpen, boldFontTagClose);
            sb.AppendFormat("{3}{0}Message: {5}{1}{6}{4}{2}", indent, ex.Message, Environment.NewLine, boldFontTagOpen, boldFontTagClose, redFontTagOpen, redFontTagClose);
            sb.AppendFormat("{3}{0}Source:{4} {1}{2}", indent, ex.Source, Environment.NewLine, boldFontTagOpen, boldFontTagClose);
            sb.AppendFormat("{3}{0}StackTrace:{4} {1}{2}", indent, ex.StackTrace, Environment.NewLine, boldFontTagOpen, boldFontTagClose);
            sb.AppendFormat("{3}{0}TargetSite:{4} {1}{2}", indent, ex.TargetSite, Environment.NewLine, boldFontTagOpen, boldFontTagClose);

            if (ex.Data.Count > 0)
            {
                sb.AppendFormat("{2}{0}Data:{3}{1}", indent, Environment.NewLine, boldFontTagOpen, boldFontTagClose);
                foreach (DictionaryEntry de in ex.Data)
                    sb.AppendFormat("{0}\t{1} : {2}", indent, de.Key, de.Value);
            }

            Exception innerException = ex.InnerException;

            while (innerException.IsNotNull())
            {
                sb.Append(innerException.FullInfo());
                if (ExceptionExtensions.exceptionLevel > 1)
                    innerException = innerException.InnerException;
                else
                    innerException = null;
            }

            ExceptionExtensions.exceptionLevel--;

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
}
