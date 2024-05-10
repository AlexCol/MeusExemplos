using SegundoExemplo.src.Interfaces;

namespace SegundoExemplo.src.Model;

public class SingletonService : IService {
    private readonly Guid _id;

    public SingletonService() {
        _id = Guid.NewGuid();
    }

    public string PrintId() {
        return $"SingletonService ID: {_id}";
    }
}