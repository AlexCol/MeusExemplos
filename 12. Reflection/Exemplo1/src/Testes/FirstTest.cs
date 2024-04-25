using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exemplo1.src.Model;
using Exemplo1.src.Util;

namespace Exemplo1.src.Testes;
public static class FirstTest {
    public static void Run() {
        var pessoa = new Pessoa() { Name = "João", Idade = 20 };
        var equipamento = new Equipamento() { Descricao = "Chave de Fenda", Modelo = "F20", Marca = "Tramontina" };
        var livro = new Livro() { Titulo = "Harry Potter", Lancamento = new DateTime(1997, 6, 26) };

        Log.Present(pessoa);
        Console.WriteLine("-----------------------");
        Log.Present(equipamento);
        Console.WriteLine("-----------------------");
        Log.Present(livro);
    }
}
