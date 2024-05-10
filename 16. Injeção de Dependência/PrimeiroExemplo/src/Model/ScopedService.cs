using PrimeiroExemplo.src.Interfaces;

namespace PrimeiroExemplo.src.Model;

public class ScopedService : IService {
    private readonly Guid _id;

    public ScopedService() {
        _id = Guid.NewGuid();
    }

    public void PrintId() {
        Console.WriteLine($"ScopedService ID: {_id}");
    }
}
