using System.Collections.Generic;
using System.Linq;
using Controllers.Responses;
using Dominio;

namespace Repository;
public static class PersonagensRepository
{
    private static List<Personagem> personagens = new List<Personagem>();

    public static void Add(Personagem p)
    {
        personagens.Add(p);
    }

    public static List<ListaDePersonagensResponse>? listaPersonagens()
    {
        if (personagens == null)
            return null;

        List<ListaDePersonagensResponse> lista = personagens.Select(p =>
            new ListaDePersonagensResponse(
                p.Nome,
                p.Estatisticas.GetType().Name,
                p.Estatisticas.isAlive() ? "Alive" : "Dead"
            )).ToList();

        return lista;
    }
    public static PersonagemResponse? buscaPorId(int id)
    {
        Personagem? personagem = personagens.Where(p => p.Id == id).FirstOrDefault();
        if (personagem == null)
            return null;

        PersonagemResponse personagemResponse = new PersonagemResponse(
            personagem.Id,
            personagem.Nome,
            personagem.Estatisticas.GetType().Name,
            personagem.Estatisticas,
            personagem.Estatisticas.getFormulaAtaque(),
            personagem.Estatisticas.getFormulaVelocidade()
        );
        return personagemResponse;
    }

    public static void update(Personagem personagemAtualizar)
    {
        int? indice = personagens.FindIndex(p => p.Id == personagemAtualizar.Id);
        if (indice != null)
            personagens[(int)indice] = personagemAtualizar;
    }



    public static int novoId()
    {
        if (personagens.Count > 0)
            return personagens.Max<Personagem>(p => p.Id) + 1;
        else
            return 1;
    }


}