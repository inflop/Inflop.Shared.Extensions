using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Inflop.Shared.Extensions.Tests;

public class UriExtensionsTest
{
    [Fact]
    public void WithHttpQueryParams_Should_Return_Expected_Url_With_Params_As_Dictionary()
    {
        // Arrange
        var url = "http://localhost";
        var uri = new UriBuilder(url).Uri;

        (string name, string value) p1 = ("p1", "v1");
        (string name, string value) p2 = ("p2", "v2");
        var @params = new Dictionary<string, string>()
        {
            [p1.name] = p1.value,
            [p2.name] = p2.value
        };
        var expectedQuery = $"{url}/?{p1.name}={p1.value}&{p2.name}={p2.value}";

        // Actual
        Uri newUri = uri.WithHttpQueryParams(@params);


        // Assert
        newUri.ToString().Should().Be(expectedQuery);
    }

    [Fact]
    public void WithHttpQueryParams_Should_Return_Expected_Url_With_Params_As_Anonymous()
    {
        // Arrange
        var url = "http://localhost";
        var uri = new UriBuilder(url).Uri;

        (string name, string value) p1 = ("p1", "v1");
        (string name, string value) p2 = ("p2", "v2");
        var @params = new { p1 = p1.value, p2 = p2.value };

        var expectedQuery = $"{url}/?{p1.name}={p1.value}&{p2.name}={p2.value}";

        // Actual
        Uri newUri = uri.WithHttpQueryParams(@params);

        // Assert
        newUri.ToString().Should().Be(expectedQuery);
    }
}
