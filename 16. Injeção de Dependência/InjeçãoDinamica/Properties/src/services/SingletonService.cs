using InjeçãoDinamica.Properties.src.config.DI.enumeradores;
using InjeçãoDinamica.Properties.src.config.DI.model;

namespace InjeçãoDinamica.Properties.src.services;

public interface ISingletonService {
  Guid GetGuid();
}

[Injectable(EServiceLifetimeType.Singleton)]
public class SingletonService : ISingletonService {
  public Guid Guid { get; set; }

  public SingletonService() {
    Guid = Guid.NewGuid();
  }

  public Guid GetGuid() {
    return Guid;
  }
}
