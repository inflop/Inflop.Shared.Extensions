using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inflop.Shared.Extensions
{
    public static class UriExtensions
    {
        public static Uri WithHttpQueryParams(this Uri uri, Dictionary<string, string> @params)
        {
			var url = HttpUtility.UrlEncode($"{uri}?{string.Join("&", @params.Select(kv => $"{kv.Key}={kv.Value}"))}");

            return new Uri(url);
        }

        public static Uri WithHttpQueryParams(this Uri uri, object @params)
        {
			return uri.WithHttpQueryParams(@params.GetType().GetProperties().AsEnumerable().ToDictionary(k => k.Name, v => v.GetValue(@params, null)?.ToString()));
        }
    }
}