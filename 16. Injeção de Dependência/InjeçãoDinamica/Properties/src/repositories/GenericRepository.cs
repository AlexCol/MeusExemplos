using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.config.DI.model;

namespace InjeçãoDinamica.Properties.src.repositories;

public interface IGenericRepository<T> {
  string SayGeneric();
}

[IgnoreInjection]
public class GenericRepository<T> : IGenericRepository<T> {
  public virtual string SayGeneric() {
    return $"Generic implementation for: {GetType()}";
  }
}
