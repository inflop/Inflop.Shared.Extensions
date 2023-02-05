using System.Linq.Expressions;

namespace Inflop.Shared.Extensions;

public static class GlobalStaticMethods
{
    /// <summary>
    /// Zwraca pełną przestrzeń nazw dla właściwości klasy zawartej w przekazanym wyrażeniu.
    /// Przestrzeń nazw zawiera również nazwę tej właściwości.
    /// </summary>
    public static string GetPropertyNamespace<T>(Expression<Func<T, object>> propertyExpression)
    {
        var mbody = propertyExpression.Body as MemberExpression;

        if (mbody.IsNull())
        {
            var ubody = propertyExpression.Body as UnaryExpression;

            if (ubody.IsNotNull())
                mbody = ubody.Operand as MemberExpression;

            if (mbody.IsNull())
                throw new ArgumentException("Expression is not a MemberExpression", nameof(propertyExpression));
        }

        return $"{typeof(T).FullName}.{mbody.Member.Name}";
    }
}
