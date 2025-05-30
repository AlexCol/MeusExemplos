using System.Reflection;
using InjeçãoDinamica.Properties.src.config.DI.enumeradores;
using InjeçãoDinamica.Properties.src.config.DI.model;

namespace InjeçãoDinamica.Properties.src.config.DI.extension;

public static partial class DependencyInjectionExtensions {
    // 🔷 Processa classes com [Injectable]
    private static bool TryAddClassWithAttribute(IServiceCollection services, Type concreteType) {
        if (!concreteType.IsConcreteType())
            return false;

        var injectableAttr = concreteType.GetCustomAttribute<InjectableAttribute>();
        if (injectableAttr != null) {
            Type? resolvedInterfaceType = injectableAttr.InterfaceType
                ?? concreteType.GetInterface($"I{concreteType.Name}");

            if (resolvedInterfaceType == null) {
                Console.WriteLine($"[DI WARNING] Interface 'I{concreteType.Name}' not found for {concreteType.FullName}. Use [Injectable(typeof(IMinhaInterface))] se necessário.");
                return false;
            }

            Register(services, resolvedInterfaceType, concreteType, injectableAttr.Lifetime);
            return true;
        }

        return false;
    }
}
