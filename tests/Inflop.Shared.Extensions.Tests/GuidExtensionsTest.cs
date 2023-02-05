using System;
using FluentAssertions;
using Xunit;

namespace Inflop.Shared.Extensions.Tests;

public class GuidExtensionsTest
{
    [Fact]
    public void ToShortString_Should_Return_Correct_String()
    {
        // Arrange
        var guid = Guid.Parse("6d032b99-ad8b-43e5-aa8f-7a6b925a448d");
        var expected = "mSsDbYut5UOqj3prklpEjQ";

        // Actual
        var result = guid.ToShortString();

        // Assert
        result.Should().Be(expected);
    }
}
