using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeiroExemplo.src.Model;

public class Pessoa {
    public string Nome { get; set; }
    public string Documento { get; set; }
    public double Altura { get; set; }
    public DateTime DataNascimento { get; set; }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.Append($"Meu nome é {Nome}, ");
        builder.Append($"meu documento é {Documento}, ");
        builder.Append($"tenho {Altura.ToString("F2", CultureInfo.InvariantCulture)}m de altura e ");
        builder.Append($"nasci em {DataNascimento.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}.");
        return builder.ToString();
    }
}
