using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InjeçãoDinamica.Properties.src.services;

public interface IGenericService<T> {
  T Show(T item);
}

public class GenericService<T> : IGenericService<T> {
  public T Show(T item) {
    return item;
  }

}
