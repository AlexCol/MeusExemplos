namespace InjeçãoDinamica.Properties.src.services;

public interface IScopedService {
  Guid GetGuid();
}

public class ScopedService : IScopedService {
  public Guid Guid { get; set; }

  public ScopedService() {
    Guid = Guid.NewGuid();
  }
  public Guid GetGuid() {
    return Guid;
  }
}
