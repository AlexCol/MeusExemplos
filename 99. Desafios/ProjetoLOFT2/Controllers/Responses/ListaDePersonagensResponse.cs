namespace Controllers.Responses;
public record ListaDePersonagensResponse(
    string nome,
    string classe,
    string aliveOrDead
);