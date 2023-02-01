using Flunt.Notifications;

public abstract class BaseClass
{
    public int PontosDeVida { get; set; }
    public int Forca { get; set; }
    public int Destreza { get; set; }
    public int Inteligencia { get; set; }

    public abstract int Ataque();
    public abstract string getFormulaAtaque();
    public abstract int Velocidade();
    public abstract string getFormulaVelocidade();

}