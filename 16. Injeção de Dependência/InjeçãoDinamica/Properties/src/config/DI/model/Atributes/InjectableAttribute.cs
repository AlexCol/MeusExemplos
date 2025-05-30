using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.config.DI.enumeradores;

namespace InjeçãoDinamica.Properties.src.config.DI.model;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectableAttribute : Attribute {
  public Type? InterfaceType { get; }
  public EServiceLifetimeType Lifetime { get; }

  public InjectableAttribute(EServiceLifetimeType lifetime = EServiceLifetimeType.Scoped) {
    Lifetime = lifetime;
  }

  public InjectableAttribute(Type interfaceType, EServiceLifetimeType lifetime = EServiceLifetimeType.Scoped) {
    InterfaceType = interfaceType;
    Lifetime = lifetime;
  }
}
