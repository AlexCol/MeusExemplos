using Exemplo1.MyAttributes.Custom;

namespace Exemplo1.src.Model;

[MyCustomClassAttribut("book")]
public class Livro {
    public string Titulo { get; set; }
    public DateTime Lancamento { get; set; }

    [MyCustomMethodAttribut("publish")]
    public static void EscolhaUmNomeDeLivro(FiltroLivro novoLivro) {
        Console.WriteLine($"O nome escolhido foi: {novoLivro.Nome}");
    }
}

public class FiltroLivro {
    public string Nome { get; set; }
}