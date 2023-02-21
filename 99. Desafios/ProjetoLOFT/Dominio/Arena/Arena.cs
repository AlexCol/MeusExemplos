class Arena
{
    public Personagem Player1 { get; private set; }
    public Personagem Player2 { get; private set; }
    private int InitPlayer1 { get; set; }
    private int InitPlayer2 { get; set; }

    public Arena(Personagem p1, Personagem p2)
    {
        Player1 = p1;
        Player2 = p2;
    }

    public List<string> realizaCombate()
    {
        List<string> rodadas = new List<string>();

        rodadas.Add(rolarIniciativa());
        rodadas.AddRange(luta());

        return rodadas;
    }

    private string rolarIniciativa()
    {
        string resultado;
        Random dado = new Random();
        do
        {
            InitPlayer1 = dado.Next(Player1.Estatisticas.Velocidade() + 1);
            InitPlayer2 = dado.Next(Player2.Estatisticas.Velocidade() + 1);
        } while (InitPlayer1 == InitPlayer2);

        if (InitPlayer1 > InitPlayer2)
            resultado = $"{Player1.Nome}({InitPlayer1}) foi mais veloz que o {Player2.Nome}({InitPlayer2}), e irá começar!";
        else
            resultado = $"{Player2.Nome}({InitPlayer2}) foi mais veloz que o {Player1.Nome}({InitPlayer1}), e irá começar!";

        return resultado;
    }

    private List<string> luta()
    {
        Random dado = new Random();
        List<string> turnos = new List<string>();
        Personagem personagemAtacante = (InitPlayer1 > InitPlayer2) ? Player1 : Player2;
        Personagem personagemDefensor = (personagemAtacante == Player2) ? Player1 : Player2;

        do
        {
            int dano = dado.Next(personagemAtacante.Estatisticas.Ataque() + 1);
            personagemDefensor.Estatisticas.PontosDeVida -= dano;

            string nomeAtacante = personagemAtacante.Nome;
            string nomeDefensor = personagemDefensor.Nome;
            int vidaDefensor = personagemDefensor.Estatisticas.PontosDeVida;

            string resultadoTurno = $"{nomeAtacante} atacou {nomeDefensor} com {dano} de dano, {nomeDefensor} com {vidaDefensor} pontos de vida restantes;";
            turnos.Add(resultadoTurno);

            personagemAtacante = (personagemAtacante == Player2) ? Player1 : Player2;
            personagemDefensor = (personagemDefensor == Player2) ? Player1 : Player2;
        } while (Player1.Estatisticas.isAlive() && Player2.Estatisticas.isAlive());

        string nomeVencedor = personagemDefensor.Nome;
        int vidaDoVencedor = personagemDefensor.Estatisticas.PontosDeVida;
        string resultadoLuta = $"{nomeVencedor} venceu a batalha! {nomeVencedor} ainda tem {vidaDoVencedor} pontos de vida restantes!";
        turnos.Add(resultadoLuta);

        return turnos;
    }

}