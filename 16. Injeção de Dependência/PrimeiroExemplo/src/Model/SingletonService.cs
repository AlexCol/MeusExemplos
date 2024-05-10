using PrimeiroExemplo.src.Interfaces;

namespace PrimeiroExemplo.src.Model;

public class SingletonService : IService {
    private readonly Guid _id;

    public SingletonService() {
        _id = Guid.NewGuid();
    }

    public void PrintId() {
        Console.WriteLine($"SingletonService ID: {_id}");
    }
}