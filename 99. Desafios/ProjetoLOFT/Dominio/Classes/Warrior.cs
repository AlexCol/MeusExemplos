public class Warrior : BaseClass
{
    public Warrior()
    {
        PontosDeVida = 20;
        Forca = 20;
        Destreza = 5;
        Inteligencia = 5;
    }

    public override int Ataque()
    {
        return (int)(Forca * 0.8 + Destreza * 0.2);
    }

    public override string getFormulaAtaque()
    {
        return "80% da Força + 20% da Destreza";
    }
    public override int Velocidade()
    {
        return (int)(Destreza * 0.6 + Inteligencia * 0.2);
    }
    public override string getFormulaVelocidade()
    {
        return "60% da Destreza + 20% da Inteligência";
    }

}