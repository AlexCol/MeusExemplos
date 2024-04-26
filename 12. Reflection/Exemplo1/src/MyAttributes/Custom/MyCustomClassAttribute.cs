using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exemplo1.MyAttributes.Custom;

[AttributeUsage(AttributeTargets.Class)]
public class MyCustomClassAttribut : Attribute { //!omitido o e de attribute no nome pois senão o C# mostra sem essa palavra (sendo omitida)
    public string Descricao { get; set; }

    public MyCustomClassAttribut(string descricao) {
        Descricao = descricao;
    }
}
