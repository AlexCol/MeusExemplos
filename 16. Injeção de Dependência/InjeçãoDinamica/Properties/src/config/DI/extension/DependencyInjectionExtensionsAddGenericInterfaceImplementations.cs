using InjeçãoDinamica.Properties.src.config.DI.enumeradores;

namespace InjeçãoDinamica.Properties.src.config.DI.extension;

public static partial class DependencyInjectionExtensions {
    // 🔷 Processa implementações de interfaces genéricas fechadas
    private static bool TryAddGenericInterfaceImplementations(IServiceCollection services, Type concreteType) {
        if (!concreteType.IsConcreteType())
            return false;

        var injected = false;

        var interfaces = concreteType.GetInterfaces()
            .Where(i => i.IsGenericType && !i.IsGenericTypeDefinition);

        foreach (var iface in interfaces) {
            Register(services, iface, concreteType, EServiceLifetimeType.Scoped);
            injected = true;
        }

        return injected;
    }
}
