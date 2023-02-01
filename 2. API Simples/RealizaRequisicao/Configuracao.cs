using Microsoft.Extensions.Configuration;

public static class Configuracao
{
    public static PegaPlantaoConfiguration getConfiguracao()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        //essa rotina converte o bloco de filhos de PegaPlantao e converte em objeto do tipo PegaPlantaoConfiguration mapeando os campos
        //do JSON com os campos da Classe (tem q ser o mesmo nome, se nao encontrar nao alimenta)
        var pegaPlantaoConfiguration = configuration.GetRequiredSection("PegaPlantao").Get<PegaPlantaoConfiguration>();

        return pegaPlantaoConfiguration;
    }
}