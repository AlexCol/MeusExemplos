using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ExemploEntityFrameworkWebApi.src.models;

namespace ExemploEntityFrameworkWebApi.src.util;

public static class CriteriaConverter {
  public static List<Tuple<string, object, object>> ConvertJsonElementToCriteria(JsonElement json) {
    var tuples = new List<Tuple<string, object, object>>();

    foreach (var property in json.EnumerateObject()) {
      var key = property.Name;
      var value = property.Value;

      var a = "26-06-1985";
      var c = DateTime.TryParse(a, out var b);

      if (value.ValueKind == JsonValueKind.Array) {
        var arrayValues = new List<object>();
        foreach (var item in value.EnumerateArray()) {
          arrayValues.Add(GetTypedItem(item)); // Ajuste o tipo conforme necessário
        }

        if (arrayValues.Count >= 2) {
          tuples.Add(Tuple.Create(key, arrayValues[0], arrayValues[1]));
        } else if (arrayValues.Count == 1) {
          tuples.Add(Tuple.Create(key, arrayValues[0], (object)null));
        }
      } else {
        tuples.Add(Tuple.Create(key, GetTypedItem(value), (object)null));
      }
    }
    return tuples;
  }

  private static Object GetTypedItem(JsonElement item) {
    if (item.ValueKind == JsonValueKind.String) {
      if (DateTime.TryParse(item.GetString(), out DateTime date)) return date;
      return item.GetString();
    }
    return null;
  }
}