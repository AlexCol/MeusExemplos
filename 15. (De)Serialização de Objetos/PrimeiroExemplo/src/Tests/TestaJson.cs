using PrimeiroExemplo.src.Conversores;
using PrimeiroExemplo.src.Model;

namespace PrimeiroExemplo.src.Tests;

public static class TestaJson {
    public static void Run() {
        Pessoa pessoa = new Pessoa {
            Nome = "Alexandre",
            Documento = "041.153.358-53",
            Altura = 1.80,
            DataNascimento = new DateTime(1985, 06, 26)
        };

        var jSon = MyJson.ObjToJson(pessoa);
        Console.WriteLine($"Serializado para json: {jSon}");

        var novaPessoa = MyJson.JsonToObj<Pessoa>(jSon);
        Console.WriteLine($"Deserializado de json: {novaPessoa}");

    }
}
