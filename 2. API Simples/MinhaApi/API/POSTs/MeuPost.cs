using Microsoft.AspNetCore.Mvc;

public class MeuPost
{
    public static string Template => "/meupost"; //https://localhost:7157/meupost
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(
        UsuarioRequest usuario //recebe do corpo da requisicao, automaticamente mapeando os nomes do obj (record ou classe) de acordo com o que encontrado no json no corpo da requisição
    )
    {
        /*
        post usado normalmente para se salvar o objeto enviado em banco
        */
        return Results.Ok(usuario.ToString());
    }
}