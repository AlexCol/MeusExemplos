public static class ResponseExtensions
{
    public static Personagem converteParaPersonagem(this PersonagemResponse p)
    {
        BaseClass novaClasse = p.classe.retornaClasse();
        Personagem personagemConvertido = new Personagem(p.id, p.nome, novaClasse);

        personagemConvertido.Estatisticas.PontosDeVida = p.Estatisticas.PontosDeVida;
        personagemConvertido.Estatisticas.Forca = p.Estatisticas.Forca;
        personagemConvertido.Estatisticas.Inteligencia = p.Estatisticas.Inteligencia;
        personagemConvertido.Estatisticas.Destreza = p.Estatisticas.Destreza;

        return personagemConvertido;
    }
}