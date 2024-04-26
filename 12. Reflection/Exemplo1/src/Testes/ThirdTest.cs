using System.Reflection;

namespace Exemplo1.src.Testes;
public class ThirdTest {
    public static void Run() {
        var ourAssembly = Assembly.GetExecutingAssembly();
        var allTypes = ourAssembly.GetTypes();
        var types = allTypes.ToArray();

        Console.WriteLine($"Found {types.Length} types!");

        for (int i = 0; i < types.Length; i++) {
            Console.WriteLine($"Type {i}: {types[i].Name}");
        }
    }
}