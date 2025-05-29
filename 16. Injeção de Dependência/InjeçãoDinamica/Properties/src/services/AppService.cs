using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.repositories;

namespace InjeçãoDinamica.Properties.src.services;

public interface IAppService {
  string SayHello();
}

public class AppService : IAppService {
  private readonly IAppRepository _repository;

  public AppService(IAppRepository repository) {
    _repository = repository;
  }

  public string SayHello() {
    return _repository.SayHello();
  }
}
