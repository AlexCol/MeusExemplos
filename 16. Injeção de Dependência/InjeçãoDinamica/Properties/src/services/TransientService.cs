using InjeçãoDinamica.Properties.src.config.DI.enumeradores;
using InjeçãoDinamica.Properties.src.config.DI.model;

namespace InjeçãoDinamica.Properties.src.services;

public interface ITransientService {
  Guid GetGuid();
}

[Injectable(typeof(ITransientService), lifetime: EServiceLifetimeType.Transient)]
public class TransientService : ITransientService {
  public Guid Guid { get; set; }

  public TransientService() {
    Guid = Guid.NewGuid();
  }

  public Guid GetGuid() {
    return Guid;
  }
}
