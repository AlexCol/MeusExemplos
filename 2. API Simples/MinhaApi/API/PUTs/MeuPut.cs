using Microsoft.AspNetCore.Mvc;

public class MeuPut
{
    public static string Template => "/meuput/{id}"; //https://localhost:7157/meuput/1
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(
        [FromRoute] int id,
        UsuarioRequest usuario //recebe do corpo da requisicao, automaticamente mapeando os nomes do obj (record ou classe) de acordo com o que encontrado no json no corpo da requisição
    )
    {
        /*
        post usado normalmente para se atualizar o objeto enviado em banco
        */
        return Results.Ok(id.ToString() + " " + usuario.ToString());
    }
}