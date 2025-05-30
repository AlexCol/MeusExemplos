using System.Reflection;
using InjeçãoDinamica.Properties.src.config.DI.enumeradores;
using InjeçãoDinamica.Properties.src.config.DI.model;

namespace InjeçãoDinamica.Properties.src.config.DI.extension;

public static partial class DependencyInjectionExtensions {
  // 🔷 Processa classes genéricas abertas
  private static bool TryAddGenericClass(IServiceCollection services, Type genericType) {
    if (!IsGenericTypeDefinition(genericType))
      return false;

    var injectableAttr = genericType.GetCustomAttribute<InjectableAttribute>();
    if (injectableAttr != null) {
      Type? resolvedInterfaceType = injectableAttr.InterfaceType
          ?? genericType.GetInterface($"I{genericType.Name}");

      if (resolvedInterfaceType == null) {
        Console.WriteLine($"[DI WARNING] Interface 'I{genericType.Name}' not found for {genericType.FullName}. Use [Injectable(typeof(IMinhaInterface))] se necessário.");
        return false;
      }

      RegisterGeneric(services, resolvedInterfaceType, genericType, injectableAttr.Lifetime);
      return true;
    }

    var interfaceType = FindMatchingGenericInterface(genericType);
    if (interfaceType == null) {
      Console.WriteLine($"[DI WARNING] Generic interface 'I{genericType.Name}' not found for {genericType.FullName}. Use [Injectable] if intended.");
      return false;
    }

    RegisterGeneric(services, interfaceType, genericType, EServiceLifetimeType.Scoped);
    return true;
  }
}
