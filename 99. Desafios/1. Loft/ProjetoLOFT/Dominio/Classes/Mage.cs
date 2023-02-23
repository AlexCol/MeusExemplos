public class Mage : BaseClass
{
    public Mage()
    {
        PontosDeVida = 12;
        Forca = 5;
        Destreza = 6;
        Inteligencia = 10;
    }

    public override int Ataque()
    {
        return (int)(Forca * 0.2 + Destreza * 0.5 + 1.5 * Inteligencia);
    }

    public override string getFormulaAtaque()
    {
        return "20% da Força + 50% da Destreza + 150% da Inteligência";
    }
    public override int Velocidade()
    {
        return (int)(Forca * 0.2 + Destreza * 0.5);
    }
    public override string getFormulaVelocidade()
    {
        return "20% da Força + 50% da Destreza";
    }

}