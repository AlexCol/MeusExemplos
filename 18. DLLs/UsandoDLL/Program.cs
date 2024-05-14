
using System.Globalization;
using CriandoDLL;

var result = Matematica.Soma(67, 66);
var praImprimir = result.ToString("F2", CultureInfo.InvariantCulture);
praImprimir.PrintNoConsole();