using System;
using System.Reflection;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace Inflop.Shared.Extensions.Tests
{
    public class AssemblyExtensionsTest
    {
        [Fact]
        public void TryGetAssemblyVersion_Should_Return_Version_From_GetName_Method()
        {
            // Arrange
            var version = "1.1.1";
            var assembly = Substitute.For<Assembly>();

            // Actual
            assembly.GetName()?.Version.Returns(new Version(version));

            // Assert
            //assembly.TryGetAssemblyVersion().Should().Be(version);
        }
    }
}