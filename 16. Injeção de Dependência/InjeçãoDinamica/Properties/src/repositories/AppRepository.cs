namespace InjeçãoDinamica.Properties.src.repositories;

public interface IAppRepository {
  string SayHello();
}

public class AppRepository : IAppRepository {
  public string SayHello() {
    return "Hello World";
  }
}
