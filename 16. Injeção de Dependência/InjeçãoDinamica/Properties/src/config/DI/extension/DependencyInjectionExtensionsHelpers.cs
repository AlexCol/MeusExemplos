using System.Reflection;
using System.Runtime.CompilerServices;
using InjeçãoDinamica.Properties.src.config.DI.model;

namespace InjeçãoDinamica.Properties.src.config.DI.extension;

public static partial class DependencyInjectionExtensions {
    // 🔍 Verifica se o tipo já foi registrado
    private static bool IsAlreadyRegistered(IServiceCollection services, Type serviceType, Type? implementationType = null) {
        return services.Any(sd =>
            sd.ServiceType == serviceType &&
            (implementationType == null || sd.ImplementationType == implementationType));
    }

    // 🔍 Busca interface genérica aberta correspondente
    private static Type? FindMatchingGenericInterface(Type type) {
        var targetInterfaceName = $"I{type.Name.Split('`')[0]}";

        var match = type.GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition().Name.Split('`')[0] == targetInterfaceName);

        return match?.GetGenericTypeDefinition();
    }

    // ✅ Helpers
    private static bool IsCompilerGenerated(this Type type) {
        return type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false).Any();
    }

    private static bool HasIgnoreAttribute(this Type type) {
        return type.GetCustomAttribute<IgnoreInjectionAttribute>(inherit: false) != null;
    }

    private static bool HasAttribute<T>(this Type type) where T : Attribute {
        return type.GetCustomAttribute<T>() != null;
    }

    private static bool IsInjectableType(Type type) {
        if (type.HasIgnoreAttribute())
            return false;

        if (typeof(Delegate).IsAssignableFrom(type))
            return false;

        var hasInjectable = type.HasAttribute<InjectableAttribute>();
        var cleanName = type.Name.Split('`')[0];
        var nameMatches = cleanName.EndsWith("Service") || cleanName.EndsWith("Repository");

        return hasInjectable || nameMatches;
    }

    private static bool IsGenericTypeDefinition(this Type type) {
        return type.IsGenericType && type.IsGenericTypeDefinition;
    }

    private static bool IsConcreteType(this Type type) {
        return type.IsClass && !type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition;
    }
}
