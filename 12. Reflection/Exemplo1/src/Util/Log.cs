using System.Text;

namespace Exemplo1.src.Util {
    public static class Log {
        public static void Present(object obj) {
            var tipo = obj.GetType();

            var builder = new StringBuilder();
            builder.AppendLine("Data do log: " + DateTime.Now);

            foreach (var p in tipo.GetProperties()) {
                builder.AppendLine($"{p.Name} : {p.GetValue(obj)}");
            }

            Console.WriteLine(builder.ToString());
        }
    }
}