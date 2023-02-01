public static class PersonagensRepository
{
    private static List<Personagem> personagens = new List<Personagem>();

    public static void Add(Personagem p)
    {
        personagens.Add(p);
    }

    public static List<PersonagemResponse>? listaPersonagens()
    {
        if (personagens == null)
            return null;

        List<PersonagemResponse> lista = new List<PersonagemResponse>();
        foreach (Personagem p in personagens)
        {
            lista.Add(
                new PersonagemResponse(
                            p.Nome,
                            p.Estatisticas.GetType().ToString(),
                            p.Estatisticas,
                            p.Estatisticas.getFormulaAtaque(),
                            p.Estatisticas.getFormulaVelocidade()
                        )
            );
        }
        return lista;
    }
    public static PersonagemResponse? buscaPorId(int id)
    {
        Personagem? personagem = personagens.Where(p => p.Id == id).FirstOrDefault();
        if (personagem == null)
            return null;

        PersonagemResponse personagemResponse = new PersonagemResponse(
            personagem.Nome,
            personagem.Estatisticas.GetType().ToString(),
            personagem.Estatisticas,
            personagem.Estatisticas.getFormulaAtaque(),
            personagem.Estatisticas.getFormulaVelocidade()
        );
        return personagemResponse;
    }

    public static void update(Personagem p)
    {

    }



    public static int novoId()
    {
        if (personagens.Count > 0)
            return personagens.Max<Personagem>(p => p.Id) + 1;
        else
            return 1;
    }


}