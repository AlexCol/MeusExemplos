using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exemplo1.MyAttributes.Custom;

namespace Exemplo1.src.Model;

[MyCustomClassAttribut("tools")]
public class Equipamento {
    public string Descricao { get; set; }
    public string Modelo { get; set; }
    public string Marca { get; set; }

    [MyCustomMethodAttribut("pedido")]
    public static void MarcaPreferida(string marca) {
        Console.WriteLine($"Minha marca preferida é: {marca}");
    }
}