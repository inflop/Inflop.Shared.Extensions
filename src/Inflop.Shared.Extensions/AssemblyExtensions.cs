using System.Reflection;

namespace Inflop.Shared.Extensions;

public static class AssemblyExtensions
{
    public static Version DefaultVersion => new Version("1.0.0");

    public static string TryGetAssemblyVersion(this Assembly assembly)
    {
        var assemblyFileVersionAttribute = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true).FirstOrDefault() as AssemblyFileVersionAttribute;
        var version = assemblyFileVersionAttribute?.Version ?? "";

        if (version.IsEmpty())
            version = assembly?.GetName()?.Version?.ToString();

        if (version.IsEmpty())
            version = DefaultVersion.ToString();

        return version;
    }

    public static string TryGetAssemblyName(this Assembly assembly)
        => assembly.GetName()?.Name ?? "";
}
