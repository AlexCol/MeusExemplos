using System.Reflection;

namespace Exemplo1.src.Testes;
public static class SecondTest {
    public static void Run() {
        var ourAssembly = Assembly.GetExecutingAssembly();
        var allTypes = ourAssembly.GetTypes();

        while (true) {
            Console.WriteLine("Enter the substring of th type to look for (or enter with empty line to exit):");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) {
                break;
            }

            var types = allTypes
                                .Where(x => x.Name.Contains(line, StringComparison.OrdinalIgnoreCase))
                                .ToArray();

            Console.WriteLine($"Found {types.Length} types!");

            for (int i = 0; i < types.Length; i++) {
                Console.WriteLine($"Type {i}: {types[i].Name}");

                Console.WriteLine("Constructors:");
                var constructors = types[i].GetConstructors();
                for (var c = 0; c < constructors.Length; c++) {
                    var constructor = constructors[c];

                    Console.WriteLine($"    Constructor {c}: {constructor.Name}");
                    foreach (var parameter in constructor.GetParameters()) {
                        Console.WriteLine($"        Parameter: {parameter.Name} - {parameter.ParameterType}");
                    }
                }

                Console.WriteLine("Methods:");
                var methods = types[i].GetMethods();
                for (var m = 0; m < methods.Length; m++) {
                    var method = methods[m];
                    Console.WriteLine($"    Method {m}: {method.Name}");
                    foreach (var parameter in method.GetParameters()) {
                        Console.WriteLine($"        Parameter: {parameter.Name} - {parameter.ParameterType}");
                    }
                }

                Console.WriteLine("Fields:");
                var fields = types[i].GetFields();
                for (var f = 0; f < fields.Length; f++) {
                    var field = fields[f];
                    Console.WriteLine($"    Field {f}: {field.Name} - {field.FieldType}");
                }

                Console.WriteLine("Properties:");
                var properties = types[i].GetProperties();
                for (var p = 0; p < properties.Length; p++) {
                    var property = properties[p];
                    Console.WriteLine($"    Property {p}: {property.Name} - {property.PropertyType}");
                }

            }
        }
    }
}