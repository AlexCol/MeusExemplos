using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.config.DI.enumeradores;
using InjeçãoDinamica.Properties.src.config.DI.model;
using InjeçãoDinamica.Properties.src.model;
using InjeçãoDinamica.Properties.src.repositories;

namespace InjeçãoDinamica.Properties.src.services;

public interface IUsuarioService : IGenericRepository<Usuario> {

}

public class UsuarioService : GenericRepository<Usuario>, IUsuarioService {
  public override string SayGeneric() {
    return "Im a client!";
  }
}
