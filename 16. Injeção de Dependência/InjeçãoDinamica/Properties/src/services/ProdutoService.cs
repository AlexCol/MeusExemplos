using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InjeçãoDinamica.Properties.src.config.DI.enumeradores;
using InjeçãoDinamica.Properties.src.config.DI.model;
using InjeçãoDinamica.Properties.src.model;
using InjeçãoDinamica.Properties.src.repositories;

namespace InjeçãoDinamica.Properties.src.services;

public interface IProdutoService : IGenericRepository<Produto> {

}

public class ProdutoService : GenericRepository<Produto>, IProdutoService {

}
