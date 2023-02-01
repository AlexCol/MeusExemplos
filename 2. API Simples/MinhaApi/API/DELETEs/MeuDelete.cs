using Microsoft.AspNetCore.Mvc;

public class MeuDelete
{
    public static string Template => "/meudelete/{id}"; //https://localhost:7157/meudelete/1
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] int id)
    {
        /*
        utilizado normalmente pra deletar informação do banco
        */
        return Results.Ok(id.ToString());
    }
}