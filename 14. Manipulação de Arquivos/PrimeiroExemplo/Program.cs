using System.Text;
using PrimeiroExemplo.src.Util;


string fileName = "teste.txt";
StringBuilder builder = new StringBuilder();
builder.AppendLine("Linha1");
builder.AppendLine("Linha2");


ManipArquivos manip = new ManipArquivos(fileName);
manip.EscreveEmArquivo(builder.ToString());

var textSalvo = manip.LeDeArquivo();
Console.WriteLine(textSalvo);
Console.ReadLine();

manip.MoveArquivo("./");
Console.ReadLine();

manip.CopiaArquivo("./src/Files/Copia/");
Console.ReadLine();

manip.ExcluiArquivo();