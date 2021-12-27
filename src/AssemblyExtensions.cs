using System.Reflection;

namespace Inflop.Shared.Extensions
{
    public static class AssemblyExtensions
    {
        public static string TryGetAssemblyVersion(this Assembly assembly)
        {
            var version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            if (version.IsEmpty())
            {
                version = assembly?.GetName()?.Version.ToString();
            }

            if (version.IsEmpty())
            {
                version = "1.0.0";
            }

            return version;
        }

        public static string TryGetAssemblyName(this Assembly assembly)
            => assembly.GetName()?.Name;
    }
}