using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploEntityFrameworkWebApi.src.models.search;

public class SearchCriteria {
  public string Key { get; set; }
  public object Value1 { get; set; }
  public object Value2 { get; set; }
  public bool IsNegated { get; set; }
  public bool IsIn { get; set; }

  public SearchCriteria(string key, object value1) {
    Key = key;
    Value1 = value1;
    Value2 = null;
    IsNegated = false;
    IsIn = false;
  }

  public SearchCriteria(string key, object value1, object value2, bool isNegated, bool isIn) {
    Key = key;
    Value1 = value1;
    Value2 = value2;
    IsNegated = isNegated;
    IsIn = isIn;
  }
}