using SegundoExemplo.src.Interfaces;

namespace SegundoExemplo.src.Model;

public class TransientService : IService {
    private readonly Guid _id;

    public TransientService() {
        _id = Guid.NewGuid();
    }

    public string PrintId() {
        return $"TransientService ID: {_id}";
    }
}
