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
        public void TryParseToDateTime_String_Should_Be_Parsed(string input)
        {
            var (parsed, value) = input.TryParseToDateTime();
            parsed.Should().BeTrue();
            value.Should().NotBe(DateTimeExtensions.DefaultDateTime);
        }

        [Theory]
        [InlineData("111.12:12:13 1/12/2021", "fff.ss:mm:HH d/M/yyyy")]
        [InlineData("11:12:13.111 2021/1/12", "ss:mm:HH.fff yyyy/d/M")]
        [InlineData("11:12 1/2021/12", "mm:HH d/yyyy/M")]
        public void TryParseToDateTime_String_Should_Be_Parsed_With_Format(string input, string format)
        {
            var (parsed, value) = input.TryParseToDateTime(format);
            parsed.Should().BeTrue();
            value.Should().NotBe(DateTimeExtensions.DefaultDateTime);
        }
        
        [Theory]
        [InlineData("123123123")]
        [InlineData("testest")]
        [InlineData("13.13.2021 12:20:12")]
        public void TryParseToDateTime_String_Should_Not_Be_Parsed(string input)
        {
            var (parsed, value) = input.TryParseToDateTime();
            parsed.Should().BeFalse();
            value.Should().Be(DateTimeExtensions.DefaultDateTime);
        }
        
        [Fact]
        public void ToBoolean_Should_Return_Expected_Value()
            => BooleanExtensions.BOOLEAN_MAPPING.ForEach(kv => kv.Key.ToBoolean().Should().Be(kv.Value));
        
        [Theory]
        [InlineData("123123123")]
        [InlineData("testest")]
        public void ToBoolean_Should_Return_False(string input)
            => input.ToBoolean().Should().BeFalse(); 

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\n\r")]
        [InlineData("\r")]
        [InlineData(null)]
        public void IsEmpty_Should_Return_True(string input)
            => input.IsEmpty().Should().BeTrue();
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\n\r")]
        [InlineData("\r")]
        [InlineData(null)]
        public void IsNotEmpty_Should_Return_False(string input)
            => input.IsNotEmpty().Should().BeFalse();
    }
}
