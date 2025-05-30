using InjeçãoDinamica.Properties.src.config.DI.enumeradores;

namespace InjeçãoDinamica.Properties.src.config.DI.extension;

public static partial class DependencyInjectionExtensions {
    // 🔷 Registro simples
    private static void Register(IServiceCollection services, Type interfaceType, Type implementationType, EServiceLifetimeType lifetime) {
        if (IsAlreadyRegistered(services, interfaceType, implementationType)) {
            Console.WriteLine($"[DI INFO] {implementationType.Name} já registrado como {interfaceType.Name}. Ignorando.");
            return;
        }

        var serviceDescriptor = lifetime switch {
            EServiceLifetimeType.Scoped => ServiceDescriptor.Scoped(interfaceType, implementationType),
            EServiceLifetimeType.Singleton => ServiceDescriptor.Singleton(interfaceType, implementationType),
            EServiceLifetimeType.Transient => ServiceDescriptor.Transient(interfaceType, implementationType),
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
        };

        services.Add(serviceDescriptor);
    }

    // 🔷 Registro genérico
    private static void RegisterGeneric(IServiceCollection services, Type interfaceType, Type implementationType, EServiceLifetimeType lifetime) {
        if (IsAlreadyRegistered(services, interfaceType, implementationType)) {
            Console.WriteLine($"[DI INFO] {implementationType.Name} já registrado como {interfaceType.Name}. Ignorando.");
            return;
        }

        var serviceDescriptor = lifetime switch {
            EServiceLifetimeType.Scoped => ServiceDescriptor.Scoped(interfaceType, implementationType),
            EServiceLifetimeType.Singleton => ServiceDescriptor.Singleton(interfaceType, implementationType),
            EServiceLifetimeType.Transient => ServiceDescriptor.Transient(interfaceType, implementationType),
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
        };

        services.Add(serviceDescriptor);
    }
}
