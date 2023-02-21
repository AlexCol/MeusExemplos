public class RealizaCombate
{
    public static string Template => "/realizacombate"; //https://localhost:7101/novopersonagem
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int? p1, int? p2)
    {
        if (p1 == null || p2 == null)
            return Results.BadRequest("Both players Id must me informed.");

        PersonagemResponse? playerResponse1 = PersonagensRepository.buscaPorId((int)p1);
        if (playerResponse1 == null)
            return Results.BadRequest("Personagem 1 não encontrado.");

        PersonagemResponse? playerResponse2 = PersonagensRepository.buscaPorId((int)p2);
        if (playerResponse2 == null)
            return Results.BadRequest("Personagem 2 não encontrado.");

        Personagem playe1 = playerResponse1.converteParaPersonagem();
        Personagem playe2 = playerResponse2.converteParaPersonagem();

        Arena arena = new Arena(playe1, playe2);
        List<string> retorno = arena.realizaCombate();

        PersonagensRepository.update(playe1);
        PersonagensRepository.update(playe2);

        return Results.Ok(retorno);
    }
}