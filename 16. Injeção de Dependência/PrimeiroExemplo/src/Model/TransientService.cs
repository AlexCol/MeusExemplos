using PrimeiroExemplo.src.Interfaces;

namespace PrimeiroExemplo.src.Model;

public class TransientService : IService {
    private readonly Guid _id;

    public TransientService() {
        _id = Guid.NewGuid();
    }

    public void PrintId() {
        Console.WriteLine($"TransientService ID: {_id}");
    }
}
