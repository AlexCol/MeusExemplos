

using Microsoft.Extensions.Configuration;
using RestSharp;

try
{
    //! lendo arquivo de configuração, e trazendo um objeto com os dados de configuração, com senhas e links de acesso
    var config = Configuracao.getConfiguracao();

    //! criando a API e injetando as configurações
    var api = new APIPegaPlantao(config);

    //! conectando, de modo que ela autorize e obtenha o token de autorização para futuras consultas
    api.Connect();


    //! executando uma consulta
    List<Doctor> doctors = api.GetProfissionais();
    doctors.OrderBy(d => d.Name);
    foreach (var item in doctors)
    {
        //System.Console.WriteLine($"{item.Name}, {item.ProfessionalId}, {item.InternalCode}, {item.UserType}");
    }
}
catch (Exception e)
{
    System.Console.WriteLine(e.Message);
}