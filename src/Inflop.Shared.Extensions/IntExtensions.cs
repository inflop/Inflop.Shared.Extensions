namespace Inflop.Shared.Extensions;

public static class IntExtensions
{
    public static string AsExcelColumnName(this int index)
    {
        int dividend = index;
        string columnName = string.Empty;
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (int)((dividend - modulo) / 26);
        }

        return columnName;
    }
}
