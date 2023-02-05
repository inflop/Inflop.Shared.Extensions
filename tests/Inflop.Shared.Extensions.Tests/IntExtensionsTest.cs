using FluentAssertions;
using Xunit;

namespace Inflop.Shared.Extensions.Tests;

public class IntExtensionsTest
{
    [Theory]
    [InlineData(1, "A")]
    [InlineData(2, "B")]
    [InlineData(3, "C")]
    [InlineData(4, "D")]
    [InlineData(5, "E")]
    [InlineData(6, "F")]
    [InlineData(7, "G")]
    [InlineData(8, "H")]
    [InlineData(9, "I")]
    [InlineData(10, "J")]
    [InlineData(11, "K")]
    [InlineData(12, "L")]
    [InlineData(13, "M")]
    [InlineData(14, "N")]
    [InlineData(15, "O")]
    [InlineData(16, "P")]
    [InlineData(17, "Q")]
    [InlineData(18, "R")]
    [InlineData(19, "S")]
    [InlineData(20, "T")]
    [InlineData(21, "U")]
    [InlineData(22, "V")]
    [InlineData(23, "W")]
    [InlineData(24, "X")]
    [InlineData(25, "Y")]
    [InlineData(26, "Z")]
    [InlineData(27, "AA")]
    [InlineData(28, "AB")]
    [InlineData(29, "AC")]
    [InlineData(52, "AZ")]
    [InlineData(104, "CZ")]
    [InlineData(208, "GZ")]
    public void AsExcelColumnName_Should_Return_Valid_Column_Name(int index, string columnName)
    {
        index.AsExcelColumnName().Should().Be(columnName);
    }
}
