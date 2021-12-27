using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Inflop.Shared.Extensions.Tests
{
    public class TypeExtensionsTest
    {
        [Theory]
        [InlineData(typeof(DateTime?))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(string))]
        [InlineData(typeof(TypeExtensions))]
        [InlineData(typeof(Assembly))]
        public void IsNullable_Should_Return_True(Type type)
        {
            type.IsNullable().Should().Be(true);
        }

        [Theory]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(int))]
        [InlineData(typeof(decimal))]
        public void IsNullable_Should_Return_False(Type type)
        {
            type.IsNullable().Should().Be(false);
        }

        [Theory]
        [InlineData(typeof(DateTime?), typeof(DateTime))]
        [InlineData(typeof(int?), typeof(int))]
        [InlineData(typeof(decimal?), typeof(decimal))]
        [InlineData(typeof(string), typeof(string))]
        [InlineData(typeof(TypeExtensions), typeof(TypeExtensions))]
        public void GetCoreType_Should_Return_Valid_Core_Type(Type type, Type expected)
        {
            type.GetCoreType().Should().Be(expected);
        }
    }
}
