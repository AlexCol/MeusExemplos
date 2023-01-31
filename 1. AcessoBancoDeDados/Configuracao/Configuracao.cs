using Microsoft.Extensions.Configuration;

namespace acessoBancoDeDados.Configuracao;

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