using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using ExemploEntityFrameworkWebApi.src.models;

namespace ExemploEntityFrameworkWebApi.src.util;

public static class CriteriaConverter {
  public static List<Tuple<string, object, object>> ConvertJsonElementToCriteria(JsonElement json) {
    var tuples = new List<Tuple<string, object, object>>();

    foreach (var property in json.EnumerateObject()) {
      var key = property.Name;
      var value = property.Value;

      if (value.ValueKind == JsonValueKind.Array) {
        ProcessArrayValue(key, value, tuples);
      } else {
        tuples.Add(Tuple.Create(key, GetTypedItem(key, value), (object)null));
      }
    }
    return tuples;
  }

  private static void ProcessArrayValue(string key, JsonElement value, List<Tuple<string, object, object>> tuples) {
    var arrayValues = new List<object>();
    foreach (var item in value.EnumerateArray()) {
      arrayValues.Add(GetTypedItem(key, item));
    }

    if (arrayValues.Count >= 2) {
      ValidateArrayValuesType(key, arrayValues);
      tuples.Add(Tuple.Create(key, arrayValues[0], arrayValues[1]));
    } else if (arrayValues.Count == 1) {
      tuples.Add(Tuple.Create(key, arrayValues[0], (object)null));
    }
  }

  private static void ValidateArrayValuesType(string key, List<object> arrayValues) {
    if (arrayValues[0].GetType() != arrayValues[1].GetType()) {
      throw new Exception($"CriteriaConverter - Para pesquisar entre, os tipos precisam ser iguais: {key}");
    }
  }

  private static object GetTypedItem(string key, JsonElement item) {
    if (TryCreateBaseEntityWithId(key, item, out var entity)) {
      return entity;
    }

    return item.ValueKind switch {
      JsonValueKind.String => GetStringOrDateTime(item),
      JsonValueKind.Number => GetNumericValue(item),
      JsonValueKind.False => item.GetBoolean(),
      JsonValueKind.True => item.GetBoolean(),
      _ => throw new Exception($"CriteriaConverter - Tipo de dado não identificado: {item.ValueKind}")
    };
  }

  private static bool TryCreateBaseEntityWithId(string key, JsonElement item, out _BaseEntityWithId entity) {
    entity = null;
    var type = GetBaseEntityWithIdType(key);

    if (type != null && item.TryGetInt32(out int idValue)) {
      entity = (_BaseEntityWithId)Activator.CreateInstance(type);
      entity.Id = idValue;
      return true;
    }
    return false;
  }

  private static object GetStringOrDateTime(JsonElement item) {
    if (DateTime.TryParse(item.GetString(), out DateTime date)) return date;
    return item.GetString();
  }

  private static object GetNumericValue(JsonElement item) {
    if (item.TryGetInt32(out int intValue)) return intValue;
    if (item.TryGetInt64(out long longValue)) return longValue;
    if (item.TryGetDouble(out double doubleValue)) return doubleValue;
    throw new Exception("CriteriaConverter - Número não pôde ser convertido.");
  }

  private static Type GetBaseEntityWithIdType(string key) {
    var baseType = typeof(_BaseEntityWithId);
    var assembly = Assembly.GetExecutingAssembly();

    return assembly.GetTypes()
        .FirstOrDefault(t =>
            t.Name.Equals(key, StringComparison.OrdinalIgnoreCase) &&
            baseType.IsAssignableFrom(t) &&
            !t.IsAbstract &&
            !t.IsInterface);
  }
}
