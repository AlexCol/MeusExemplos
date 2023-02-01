public class MeuGetSimples
{
    public static string Template => "/meugetsimples"; //https://localhost:7157/meugetsimples
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(
        HttpRequest http
    )
    {
        Dictionary<string, List<string>> retorno = new Dictionary<string, List<string>>();
        string linha;
        StreamReader reader;

        var path = http.Path;
        retorno.Add("Path", new List<string>());
        retorno["Path"].Add(path);

        retorno.Add("Header", new List<string>());
        var header = http.Headers;
        foreach (var item in header)
        {
            retorno["Header"].Add(item.Key + ": " + item.Value);
        }

        var body = http.Body;
        reader = new StreamReader(body);
        retorno.Add("Corpo", new List<string>());
        linha = reader.ReadLineAsync().Result;
        while (linha != null)
        {
            retorno["Corpo"].Add(linha);
            linha = reader.ReadLineAsync().Result;
        }

        return Results.Ok(retorno);
    }
}