using InjeçãoDinamica.Properties.src.config.DI.enumeradores;

namespace InjeçãoDinamica.Properties.src.config.DI.extension;

public static partial class DependencyInjectionExtensions {
    // 🔷 Processa classes por convenção de nome
    private static bool TryAddClassByConvention(IServiceCollection services, Type concreteType) {
        if (!concreteType.IsConcreteType())
            return false;

        if (concreteType.Name.EndsWith("Service") || concreteType.Name.EndsWith("Repository")) {
            var interfaceType = concreteType.GetInterface($"I{concreteType.Name}");

            if (interfaceType == null) {
                Console.WriteLine($"[DI WARNING] Interface 'I{concreteType.Name}' not found for {concreteType.FullName}. Use [Injectable] if intended.");
                return false;
            }

            Register(services, interfaceType, concreteType, EServiceLifetimeType.Scoped);
            return true;
        }

        return false;
    }
}
