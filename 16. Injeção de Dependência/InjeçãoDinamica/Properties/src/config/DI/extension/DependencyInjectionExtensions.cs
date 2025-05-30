using System.Reflection;
using System.Runtime.CompilerServices;
using InjeçãoDinamica.Properties.src.config.DI.enumeradores;
using InjeçãoDinamica.Properties.src.config.DI.model;
using Microsoft.Extensions.DependencyInjection;

namespace InjeçãoDinamica.Properties.src.config.DI.extension;

public static class DependencyInjectionExtensions {
  public static IServiceCollection AddAutoInjectables(this IServiceCollection services, params Assembly[] assemblies) {
    if (assemblies == null || assemblies.Length == 0)
      assemblies = AppDomain.CurrentDomain.GetAssemblies();

    var allTypes = assemblies
      .SelectMany(a => a.GetTypes())
      .Where(t =>
          t.IsClass &&
          !t.IsAbstract &&
          !t.IsCompilerGenerated() &&
          t.Namespace != null);

    var genericTypes = allTypes.Where(t => t.IsGenericTypeDefinition);
    var concreteTypes = allTypes.Where(t => !t.IsGenericTypeDefinition);

    AdicionaClassesGenericas(services, genericTypes);
    AdicionaImplementacoesDeInterfacesGenericas(services, concreteTypes);
    AdicionaClassesComAtributo(services, concreteTypes);
    AdicionaClassesPorConvencao(services, concreteTypes);

    return services;
  }

  // 🔷 Processa classes genéricas abertas (ex.: CrudService<T>)
  private static void AdicionaClassesGenericas(IServiceCollection services, IEnumerable<Type> genericTypes) {
    foreach (var type in genericTypes) {
      if (type.HasIgnoreAttribute())
        continue;

      var injectableAttr = type.GetCustomAttribute<InjectableAttribute>();
      if (injectableAttr != null) {
        Type? resolvedInterfaceType = injectableAttr.InterfaceType;
        if (resolvedInterfaceType == null)
          resolvedInterfaceType = type.GetInterface($"I{type.Name}");
        if (resolvedInterfaceType == null)
          throw new InvalidOperationException($"Interface 'I{type.Name}' not found for {type.FullName}. Use [Injectable(typeof(IMinhaInterface))] se necessário.");
        RegisterGeneric(services, resolvedInterfaceType, type, injectableAttr.Lifetime);
        continue;
      }

      var interfaceType = FindMatchingGenericInterface(type);
      if (interfaceType != null) {
        RegisterGeneric(services, interfaceType, type, EServiceLifetimeType.Scoped);
      } else {
        throw new InvalidOperationException(
            $"Generic interface 'I{type.Name}' not found for {type.FullName}. Use [Injectable] if intended.");
      }
    }
  }

  // 🔷 Processa implementações de interfaces genéricas fechadas (ex.: ICrudService<Usuario> → UsuarioService)
  // Adiciona um aviso se houver múltiplas implementações para a mesma interface
  private static void AdicionaImplementacoesDeInterfacesGenericas(IServiceCollection services, IEnumerable<Type> concreteTypes) {
    var interfaceToTypes = new Dictionary<Type, List<Type>>();
    foreach (var type in concreteTypes) {
      if (type.HasIgnoreAttribute())
        continue;

      var interfaces = type.GetInterfaces()
        .Where(i => i.IsGenericType && !i.IsGenericTypeDefinition);

      foreach (var iface in interfaces) {
        if (!interfaceToTypes.ContainsKey(iface))
          interfaceToTypes[iface] = new List<Type>();
        interfaceToTypes[iface].Add(type);
        Register(services, iface, type, EServiceLifetimeType.Scoped);
      }
    }
    // Detecta ambiguidade
    foreach (var kvp in interfaceToTypes) {
      if (kvp.Value.Count > 1) {
        Console.WriteLine($"[DI WARNING] Interface '{kvp.Key}' possui múltiplas implementações: {string.Join(", ", kvp.Value.Select(t => t.Name))}. Considere usar IEnumerable<T> na injeção ou ajustar o registro.");
      }
    }
  }

  // 🔷 Processa classes concretas que possuem [Injectable]
  private static void AdicionaClassesComAtributo(IServiceCollection services, IEnumerable<Type> concreteTypes) {
    foreach (var type in concreteTypes) {
      if (type.HasIgnoreAttribute())
        continue;

      var injectableAttr = type.GetCustomAttribute<InjectableAttribute>();
      if (injectableAttr != null) {
        Type? resolvedInterfaceType = injectableAttr.InterfaceType;
        if (resolvedInterfaceType == null)
          resolvedInterfaceType = type.GetInterface($"I{type.Name}");
        if (resolvedInterfaceType == null)
          throw new InvalidOperationException($"Interface 'I{type.Name}' not found for {type.FullName}. Use [Injectable(typeof(IMinhaInterface))] se necessário.");
        Register(services, resolvedInterfaceType, type, injectableAttr.Lifetime);
      }
    }
  }

  // 🔷 Processa classes concretas por convenção de nome (Service, Repository)
  private static void AdicionaClassesPorConvencao(IServiceCollection services, IEnumerable<Type> concreteTypes) {
    foreach (var type in concreteTypes) {
      if (type.HasIgnoreAttribute())
        continue;

      if (type.HasAttribute<InjectableAttribute>())
        continue; // Já foi processado no método anterior

      if (type.Name.EndsWith("Service") || type.Name.EndsWith("Repository")) {
        var interfaceType = type.GetInterface($"I{type.Name}");

        if (interfaceType != null) {
          Register(services, interfaceType, type, EServiceLifetimeType.Scoped);
        } else {
          throw new InvalidOperationException(
              $"Interface 'I{type.Name}' not found for {type.FullName}. Use [Injectable] if intended.");
        }
      }
    }
  }

  // 🔷 Registro simples
  private static void Register(IServiceCollection services, Type interfaceType, Type implementationType, EServiceLifetimeType lifetime) {
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
    var serviceDescriptor = lifetime switch {
      EServiceLifetimeType.Scoped => ServiceDescriptor.Scoped(interfaceType, implementationType),
      EServiceLifetimeType.Singleton => ServiceDescriptor.Singleton(interfaceType, implementationType),
      EServiceLifetimeType.Transient => ServiceDescriptor.Transient(interfaceType, implementationType),
      _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
    };

    services.Add(serviceDescriptor);
  }

  // 🔍 Busca interface genérica aberta correspondente
  private static Type? FindMatchingGenericInterface(Type type) {
    var targetInterfaceName = $"I{type.Name.Split('`')[0]}";

    // Busca por interface genérica aberta mesmo que implementada como fechada
    var match = type.GetInterfaces()
      .FirstOrDefault(i =>
        i.IsGenericType &&
        i.GetGenericTypeDefinition().Name.Split('`')[0] == targetInterfaceName
      );
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
}
