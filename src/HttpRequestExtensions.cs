using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Inflop.Shared.Extensions;

public static class HttpRequestExtensions
{
    public static readonly string TemporaryPasswordHeaderKeyName = "Temp-Hash";

    public static string GetHeaderValue(this HttpRequest request, string key)
    {
        string value = string.Empty;

        if (request?.Headers?.Count <= 0)
            return value;

        var values = StringValues.Empty;
        if (request.Headers.TryGetValue(key, out values))
        {
            if (values.Any())
                value = values.SingleOrDefault() ?? "";
        }

        return value;
    }

    public static string GetHeaderAuthorizationValue(this HttpRequest request)
        => request.GetHeaderValue("Authorization");
}
