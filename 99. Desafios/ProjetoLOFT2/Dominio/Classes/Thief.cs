namespace Dominio.Classes;

public class Thief : BaseClass
{
    public Thief()
    {
        PontosDeVida = 15;
        Forca = 4;
        Destreza = 10;
        Inteligencia = 4;
    }

    public override int Ataque()
    {
        return (int)(Forca * 0.25 + Destreza * 1 + 0.25 * Inteligencia);
    }

    public override string getFormulaAtaque()
    {
        return "25% da Força + 100% da Destreza + 25% da Inteligência";
    }
    public override int Velocidade()
    {
        return (int)(Destreza * 0.8);
    }
    public override string getFormulaVelocidade()
    {
        return "80% da Destreza";
    }

}