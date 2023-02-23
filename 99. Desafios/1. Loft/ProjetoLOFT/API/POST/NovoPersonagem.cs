public class NovoPersonagem
{
    public static string Template => "/novopersonagem"; //https://localhost:7101/novopersonagem
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(PersonagemRequest personagemRequest)
    {
        try
        {
            BaseClass novaClasse = personagemRequest.classe.retornaClasse();
            int novoId = PersonagensRepository.novoId();
            Personagem novoPersonagem = new Personagem(novoId, personagemRequest.nome, novaClasse);

            if (!novoPersonagem.IsValid)
            {
                var errors = novoPersonagem.Notifications.ConverteParaProblemDetails();
                return Results.ValidationProblem(errors);
            }

            PersonagensRepository.Add(novoPersonagem);

            return Results.Created($"/personagem/{novoId}", novoId);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }
}