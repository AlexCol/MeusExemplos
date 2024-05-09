using PrimeiroExemplo.src.Conversores;
using PrimeiroExemplo.src.Model;

namespace PrimeiroExemplo.src.Tests;

public class TestaXml {
    public static void Run() {
        Pessoa pessoa = new Pessoa {
            Nome = "Bernard",
            Documento = "041.153.358-53",
            Altura = 1.79,
            DataNascimento = new DateTime(1990, 09, 30)
        };

        var xml = MyXml.ObjToXml(pessoa);
        Console.WriteLine("Serializado para xml: ");
        Console.WriteLine(xml);

        var novaPessoa = MyXml.XmlToObj<Pessoa>(xml);
        Console.WriteLine($"Desserializado de xml: {novaPessoa}");
    }
}
