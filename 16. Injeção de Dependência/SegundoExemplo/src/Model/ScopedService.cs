using SegundoExemplo.src.Interfaces;

namespace SegundoExemplo.src.Model;

public class ScopedService : IService {
    private readonly Guid _id;

    public ScopedService() {
        _id = Guid.NewGuid();
    }

    public string PrintId() {
        return $"ScopedService ID: {_id}";
    }
}
