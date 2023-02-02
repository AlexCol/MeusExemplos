using Microsoft.AspNetCore.Mvc;

public class ListaPersonagens
{
    public static string Template => "/listapersonagem"; //https://localhost:7101/listapersonagem
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action()
    {
        return Results.Ok(PersonagensRepository.listaPersonagens());
    }
}