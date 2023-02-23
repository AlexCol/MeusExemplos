using Dominio.Classes;

namespace Controllers.Responses;

public record PersonagemResponse(
    int id,
    string nome,
    string classe,
    BaseClass Estatisticas,
    string Ataque,
    string Velocidade
);