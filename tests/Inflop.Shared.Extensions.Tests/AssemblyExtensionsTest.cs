using System;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Inflop.Shared.Extensions.Tests;

public class AssemblyExtensionsTest
{
    [Fact]
    public void TryGetAssemblyVersion_Should_Return_Version_From_GetName_Method()
    {
        // Arrange
        var expected = "1.1.1";
        var assemblyName = new AssemblyName() { Name = "Test", Version = new Version(expected) };
        var assembly = Substitute.For<Assembly>();

        // Actual
        assembly.GetName().Returns(assemblyName);

        // Assert
        assembly.TryGetAssemblyVersion().Should().Be(expected);
    }

    [Fact]
    public void TryGetAssemblyVersion_Should_Return_Default_Version()
    {
        Substitute.For<Assembly>().TryGetAssemblyVersion().Should().Be(AssemblyExtensions.DefaultVersion.ToString());
    }

    [Fact]
    public void TryGetAssemblyName_Should_Return_Valid_Name()
    {
        // Arrange
        var expected = "Test";
        var assemblyName = new AssemblyName() { Name = expected };
        var assembly = Substitute.For<Assembly>();

        // Actual
        assembly.GetName().Returns(assemblyName);

        // Assert
        assembly.TryGetAssemblyName().Should().Be(expected);
    }

    [Fact]
    public void TryGetAssemblyName_Should_Return_Empty_Name()
    {
        Substitute.For<Assembly>().TryGetAssemblyName().Should().BeEmpty();
    }
}
