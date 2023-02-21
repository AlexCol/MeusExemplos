using Flunt.Notifications;

public abstract class BaseClass
{
    private int pontosDeVida;
    public int PontosDeVida
    {
        get
        {
            return pontosDeVida;
        }
        set
        {
            if (value < 0)
                pontosDeVida = 0;
            else
                pontosDeVida = value;
        }
    }
    public int Forca { get; set; }
    public int Destreza { get; set; }
    public int Inteligencia { get; set; }

    public abstract int Ataque();
    public abstract string getFormulaAtaque();
    public abstract int Velocidade();
    public abstract string getFormulaVelocidade();

    public bool isAlive()
    {
        return PontosDeVida > 0;
    }

}