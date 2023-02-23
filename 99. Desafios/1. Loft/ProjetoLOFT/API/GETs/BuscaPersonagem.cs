using Microsoft.AspNetCore.Mvc;

public class BuscaPersonagem
{
    public static string Template => "/personagem/{id}"; //https://localhost:7101/personagem/{id}
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] int id)
    {
        PersonagemResponse? personagem = PersonagensRepository.buscaPorId(id);

        if (personagem == null)
            return Results.BadRequest("Personagem nao existe.");

        return Results.Ok(personagem);
    }
}