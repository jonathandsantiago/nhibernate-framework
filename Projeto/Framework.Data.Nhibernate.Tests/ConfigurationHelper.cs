using Microsoft.Extensions.Configuration;

namespace Framework.Data.Nhibernate.Tests
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
