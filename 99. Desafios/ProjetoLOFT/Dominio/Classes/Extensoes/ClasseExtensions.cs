public static class ClasseExtensions
{
    public static BaseClass retornaClasse(this string classe)
    {
        switch (classe)
        {
            case "Warrior":
                return new Warrior();
            case "Mage":
                return new Mage();
            case "Thief":
                return new Thief();
        }
        throw new Exception("Class nao encontrada");
    }
}