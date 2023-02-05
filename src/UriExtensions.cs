namespace Inflop.Shared.Extensions;

public static class UriExtensions
{
    public static Uri WithHttpQueryParams(this Uri uri, Dictionary<string, string> @params)
        => new Uri($"{uri}?{string.Join("&", @params.Select(kv => $"{kv.Key}={kv.Value}"))}");

    public static Uri WithHttpQueryParams(this Uri uri, object @params)
        => uri.WithHttpQueryParams(@params.GetType().GetProperties().AsEnumerable().ToDictionary(k => k.Name, v => v.GetValue(@params, null)?.ToString()));
}
