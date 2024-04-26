using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exemplo1.MyAttributes.Custom;

namespace Exemplo1.src.Model;

[MyCustomClassAttribut("inividuo")]
public class Pessoa {
    public string Name { get; set; }
    public int Idade { get; set; }

    [MyCustomMethodAttribut("seapresentar")]
    public static void InformaDados() {
        Console.WriteLine($"Oi! Ainda não sei meu nome.");
    }
}