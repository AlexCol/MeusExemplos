using System;

namespace Dominio.Classes.Extensoes;

public static class ClasseExtensions
{
    public static BaseClass retornaClasse(this string className)
    {
        var type = Type.GetType($"Dominio.Classes.{className}");

        if (type != null && type.IsSubclassOf(typeof(BaseClass)))
        {
#pragma warning disable CS8600
#pragma warning disable CS8603
            return (BaseClass)Activator.CreateInstance(type);
#pragma warning restore CS8600
#pragma warning restore CS8603            

        }

        throw new Exception("Classe nao encontrada");
    }
}