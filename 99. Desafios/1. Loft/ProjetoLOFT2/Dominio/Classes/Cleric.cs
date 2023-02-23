namespace Dominio.Classes;
public class Cleric : BaseClass
{
    public Cleric()
    {
        PontosDeVida = 15;
        Forca = 9;
        Destreza = 5;
        Inteligencia = 8;
    }

    public override int Ataque()
    {
        return (int)(Forca * 0.8 + Destreza * 0.5 + 0.8 * Inteligencia);
    }

    public override string getFormulaAtaque()
    {
        return "80% da Força + 50% da Destreza + 80% da Inteligência";
    }
    public override int Velocidade()
    {
        return (int)(Forca * 0.4 + Destreza * 0.2);
    }
    public override string getFormulaVelocidade()
    {
        return "40% da Força + 20% da Destreza";
    }

}