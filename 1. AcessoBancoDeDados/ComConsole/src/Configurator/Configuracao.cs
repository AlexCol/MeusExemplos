using Microsoft.Extensions.Configuration;

namespace ComConsole.src.Configurator;

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
