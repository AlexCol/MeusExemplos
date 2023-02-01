public static class ClasseExtensions
{
    public static BaseClass retornaClasse(this string classe)
    {
        switch (classe)
        {
            case "Warrior":
                return new Warrior();
        }
        throw new Exception("Class nao encontrada");
    }
}