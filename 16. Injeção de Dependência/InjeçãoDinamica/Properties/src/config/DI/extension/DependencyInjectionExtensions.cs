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

    var allTypes = assemblies.SelectMany(a => a.GetTypes())
        .Where(t =>
            t.IsClass &&
            !t.IsAbstract &&
            !t.IsCompilerGenerated() && // <-- 🔥 Ignora tipos anônimos, lambdas, etc.
            t.Namespace != null // <-- 🔥 Garante que não seja tipo do sistema
        );

    // 🔸 Processa classes genéricas abertas
    var genericTypes = allTypes.Where(t => t.IsGenericTypeDefinition);

    foreach (var type in genericTypes) {
      if (type.GetCustomAttribute<IgnoreInjectionAttribute>() != null)
        continue;

      var injectableAttr = type.GetCustomAttribute<InjectableAttribute>();
      if (injectableAttr != null) {
        RegisterGeneric(services, injectableAttr.InterfaceType, type, injectableAttr.Lifetime);
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

    // 🔸 Processa classes concretas
    var concreteTypes = allTypes.Where(t => !t.IsGenericTypeDefinition);

    foreach (var type in concreteTypes) {
      if (type.GetCustomAttribute<IgnoreInjectionAttribute>() != null)
        continue;

      var injectableAttr = type.GetCustomAttribute<InjectableAttribute>();
      if (injectableAttr != null) {
        Register(services, injectableAttr.InterfaceType, type, injectableAttr.Lifetime);
        continue;
      }

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

    return services;
  }

  private static void Register(IServiceCollection services, Type interfaceType, Type implementationType, EServiceLifetimeType lifetime) {
    var serviceDescriptor = lifetime switch {
      EServiceLifetimeType.Scoped => ServiceDescriptor.Scoped(interfaceType, implementationType),
      EServiceLifetimeType.Singleton => ServiceDescriptor.Singleton(interfaceType, implementationType),
      EServiceLifetimeType.Transient => ServiceDescriptor.Transient(interfaceType, implementationType),
      _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
    };

    services.Add(serviceDescriptor);
  }

  private static void RegisterGeneric(IServiceCollection services, Type interfaceType, Type implementationType, EServiceLifetimeType lifetime) {
    var serviceDescriptor = lifetime switch {
      EServiceLifetimeType.Scoped => ServiceDescriptor.Scoped(interfaceType, implementationType),
      EServiceLifetimeType.Singleton => ServiceDescriptor.Singleton(interfaceType, implementationType),
      EServiceLifetimeType.Transient => ServiceDescriptor.Transient(interfaceType, implementationType),
      _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
    };

    services.Add(serviceDescriptor);
  }

  private static Type? FindMatchingGenericInterface(Type type) {
    var targetInterfaceName = $"I{type.Name.Split('`')[0]}";

    return type.GetInterfaces()
        .FirstOrDefault(i =>
            i.IsGenericTypeDefinition &&
            i.Name.Split('`')[0] == targetInterfaceName
        );
  }

  /// <summary>
  /// Verifica se um tipo é gerado pelo compilador (ex.: tipos anônimos, closures, etc.)
  /// </summary>
  private static bool IsCompilerGenerated(this Type type) {
    return type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false).Any();
  }
}
