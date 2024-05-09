
using System.Xml.Serialization;
using PrimeiroExemplo.src.Conversores;
using PrimeiroExemplo.src.Model;
using PrimeiroExemplo.src.Tests;

Console.WriteLine("Iniciando Json:");
TestaJson.Run();
Console.WriteLine("------------------Finalizado Json");

Console.WriteLine("");

Console.WriteLine("Iniciando Xml:");
TestaXml.Run();
Console.WriteLine("------------------Finalizado Xml");

Console.WriteLine("Criando json");
var jsonNovo = "{\"Nome\":\"Alexandre\",\"Documento\":\"041.153.358-53\",\"Altura\":1.8,\"DataNascimento\":\"1985-06-26\"}";
var pessoa = MyJson.JsonToObj<Pessoa>(jsonNovo);
var xml = MyXml.ObjToXml(pessoa);
Console.WriteLine("MOstrando xml q veio como json");
Console.WriteLine(xml);



