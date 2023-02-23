using Microsoft.Extensions.Configuration;

public static class Configuracao
{
    public static IConfigurationRoot getConfiguracao()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        return configuration;
    }
}