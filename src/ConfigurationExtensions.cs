using Microsoft.Extensions.Configuration;

namespace Inflop.Shared.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetConfiguration<T>(this IConfiguration configuration, string sectionName = "") where T : new()
        {
            var defaultConfiguration = new T();
            configuration.GetConfigurationSection<T>(sectionName).Bind(defaultConfiguration);
            return defaultConfiguration;
        }

      public static IConfigurationSection GetConfigurationSection<T>(this IConfiguration configuration, string sectionName = "") where T : new()
      {
         sectionName = sectionName.IsEmpty() ? typeof(T).Name.Replace("Settings", string.Empty) : sectionName;
         return configuration.GetSection(sectionName);
      }
    }
}