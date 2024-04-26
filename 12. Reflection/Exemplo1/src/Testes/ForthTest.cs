using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Exemplo1.MyAttributes.Custom;

namespace Exemplo1.src.Testes;

public static class ForthTest {
    public static void Run() {
        OpcoesMenu opcaoSelecionada = (OpcoesMenu)MenuInicial();
        ProcessaOpcao(opcaoSelecionada);
    }

    private static int MenuInicial() {
        int commandCode;
        bool opcaoInvalida;

        do {
            Console.WriteLine("O que deseja fazer?");
            Console.WriteLine("1 - Se apresentar;");
            Console.WriteLine("2 - Publicar um livro;");
            Console.WriteLine("3 - Dizer sua marca de ferramentas preferida;");
            Console.WriteLine("4 - Sair;");
            Console.Write("Digitar: ");
            int.TryParse(Console.ReadLine(), out commandCode);

            opcaoInvalida = commandCode < 1 || commandCode > 4;
            if (opcaoInvalida) {
                Console.WriteLine("Opção inválida.");
                Console.WriteLine("--------------------------------------------");
                continue;
            }
        } while (opcaoInvalida);
        return commandCode;
    }

    private static void ProcessaOpcao(OpcoesMenu opt) {
        if (opt == OpcoesMenu.Sair) return;

        string comando = "";

        //! Constrói o comando com base na opção selecionada
        if (opt == OpcoesMenu.Apresentar) {
            comando = "inividuo;seapresentar";
        }

        if (opt == OpcoesMenu.PublicarLivro) {
            Console.WriteLine("Qual o nome do livro(não usar ';')?");
            var nomeLivro = Console.ReadLine().Replace(";", "");
            comando = "book;publish;{\"Nome\":\"" + nomeLivro + "\"}";
        }

        if (opt == OpcoesMenu.FalarMarcaPreferida) {
            Console.WriteLine("Qual a marca preferida?");
            var marca = Console.ReadLine().Replace(";", "");
            comando = $"tools;pedido;{marca}";
        }

        ProcessaComando(comando);
    }

    private static void ProcessaComando(string comando) {
        //! Divide o comando em partes usando ';' pois foi o separador definido
        var comandoEmArray = comando.Split(';');

        var meuSistema = Assembly.GetExecutingAssembly(); //! aqui obtenho todo o conjunto
        var classesDoSistema = meuSistema.GetTypes(); //! aqui obtenho todas as classes do sistema

        //! Filtra as classes que possuem um atributo personalizado específico
        var classesComAtributoCustomizado = classesDoSistema.Where(c => c.GetCustomAttribute<MyCustomClassAttribut>() != null);
        //! das que possuem o atributo, busca a que tem a descricao informada no comando
        var classeSelecionada = classesComAtributoCustomizado.FirstOrDefault(c => c.GetCustomAttribute<MyCustomClassAttribut>().Descricao == comandoEmArray[0]);

        if (classeSelecionada != null) {
            //! Filtra os metodos que possuem um atributo personalizado específico
            var metodosComAttCustomizado = classeSelecionada.GetMethods().Where(m => m.GetCustomAttribute<MyCustomMethodAttribut>() != null);
            //! das que possuem o atributo, busca a que tem a descricao informada no comando
            var metodoEscolhido = metodosComAttCustomizado.FirstOrDefault(m => m.GetCustomAttribute<MyCustomMethodAttribut>().Descricao == comandoEmArray[1]);

            if (metodoEscolhido != null) {
                var tiposDosParametros = metodoEscolhido.GetParameters().Select(p => p.ParameterType).ToArray();
                var qtdParametros = tiposDosParametros.Length;

                if (qtdParametros == 0) {
                    //? o primeiro parametro é uma instancia, normalmente precisa ser passada via inseção de dependencia
                    //? o segundo é para os parametros, será implementado mais abaixo
                    metodoEscolhido.Invoke(null, null);
                } else {
                    var listaParametros = new List<object>();
                    var parametrosNoComando = comandoEmArray.Skip(2).ToArray(); //!pula dois, pois os dois primeiros são da classe e metodo

                    //!validando se os comandos enviados batem com a assinatura do metodo em quantidade
                    if (parametrosNoComando.Length != tiposDosParametros.Length) throw new Exception("Parametros informados a mais ou a menos que o devido.");

                    for (int i = 0; i < parametrosNoComando.Length; i++) {
                        if (tiposDosParametros[i] == typeof(string)) {
                            listaParametros.Add(parametrosNoComando[i]);
                        } else {
                            var parametro = JsonSerializer.Deserialize(parametrosNoComando[i], tiposDosParametros[i]);
                            listaParametros.Add(parametro);
                        }
                    }
                    metodoEscolhido.Invoke(null, listaParametros.ToArray());
                }
            }
        }
    }
}

public class LowerCaseNamingPolicy : JsonNamingPolicy {
    public override string ConvertName(string name) {
        return name.ToLower();
    }
}
