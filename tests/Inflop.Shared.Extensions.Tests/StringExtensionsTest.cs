using System;
using Xunit;
using FluentAssertions;
using Inflop.Shared.Extensions;

namespace Inflop.Shared.Extensions.Tests
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("12/12/2021")]
        [InlineData("1/2/2021")]
        [InlineData("1/12/2021")]
        [InlineData("11/1/2021")]
        [InlineData("11/12/2021 12:20")]
        [InlineData("11/12/2021 12:20:59")]
        [InlineData("11/12/2021 12:20:12.123")]
        [InlineData("11-12-2021")]
        [InlineData("2-12-2021")]
        [InlineData("12-1-2021")]
        [InlineData("1-1-2021")]
        [InlineData("11-12-2021 12:20")]
        [InlineData("11-12-2021 12:20:12")]
        [InlineData("2-12-2021 12:20:14")]
        [InlineData("27-2-2021 12:20:15")]
        [InlineData("11-12-2021 12:20:18")]
        [InlineData("2-12-2021 12:20:12.123")]
        [InlineData("12-2-2021 12:20:12.123")]
        [InlineData("11-12-2021 12:20:12.123")]
        [InlineData("2021-12-11")]
        [InlineData("2021-12-1")]
        [InlineData("2021-12-11 12:20")]
        [InlineData("2021-12-1 12:20")]
        [InlineData("2021-1-12 12:20")]
        [InlineData("2021-12-1 12:20:11")]
        [InlineData("2021-12-11 12:20:11")]
        [InlineData("2021-12-11 12:20:12.123")]
        [InlineData("2021-12-1 12:20:12.123")]
        [InlineData("2021-1-11 12:20:12.123")]
        [InlineData("2021/12/11")]
        [InlineData("2021/12/1")]
        [InlineData("2021/1/19")]
        [InlineData("2021/12/11 12:20")]
        [InlineData("2021/12/1 12:20")]
        [InlineData("2021/1/11 12:20")]
        [InlineData("2021/12/11 12:20:01")]
        [InlineData("2021/12/1 12:20:00")]
        [InlineData("2021/12/1 12:20:59")]
        [InlineData("2021/12/11 12:20:11")]
        [InlineData("2021/12/11 12:20:12.123")]
        [InlineData("2021/12/1 12:20:12.123")]
        [InlineData("2021/1/11 12:20:12.123")]
        [InlineData("2021.12.1")]
        [InlineData("2021.1.21")]
        [InlineData("2021.12.11")]
        [InlineData("2021.12.11 12:20")]
        [InlineData("2021.12.1 12:20")]
        [InlineData("2021.1.11 12:20")]
        [InlineData("2021.12.11 12:20:11")]
        [InlineData("2021.12.1 12:20:11")]
        [InlineData("2021.1.11 12:20:12")]
        [InlineData("2021.12.11 12:20:14")]
        [InlineData("2021.12.11 12:20:12.123")]
        [InlineData("2021.12.1 12:20:12.123")]
        [InlineData("2021.1.11 12:20:12.123")]
        [InlineData("11.12.2021")]
        [InlineData("1.12.2021")]
        [InlineData("12.1.2021")]
        [InlineData("1.12.2021 12:20")]
        [InlineData("12.1.2021 12:20")]
        [InlineData("11.12.2021 12:20")]
        [InlineData("1.12.2021 12:20:12")]
        [InlineData("12.2.2021 12:20:12")]
        [InlineData("11.12.2021 12:20:12")]
        [InlineData("11.12.2021 12:20:12.123")]
        [InlineData("1.12.2021 12:20:12.123")]
        [InlineData("12.1.2021 12:20:12.123")]
        [InlineData("20211211")]
        [InlineData("2021-12-1T12:20:12.123")]
        [InlineData("2021-1-11T12:20:12.123")]
        [InlineData("2021-12-11T12:20:12.123")]
        public void TryParseToDateTime_String_Should_Be_Parsed_And(string input)
        {
            var result = input.TryParseToDateTime();
            result.Parsed.Should().Be(true);
            result.Value.Should().NotBe(DateTimeExtensions.DefaultDateTime);
        }
        
        [Fact]
        public void ToBoolean_Should_Return_Expected_Value()
        {
            BooleanExtensions.BOOLEAN_MAPPING.ForEach(kv => kv.Key.ToBoolean().Should().Be(kv.Value));
        }
    }
}
