
using System.Reflection;
using System.Text.Json;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.search;

namespace ExemploEntityFrameworkWebApi.src.util;

public static class CriteriaConverter {
  public static List<SearchCriteria> ConvertJsonElementToCriteria(JsonElement json) {
    var criteriaList = new List<SearchCriteria>();

    foreach (var property in json.EnumerateObject()) {
      var key = property.Name;
      var value = property.Value;
      var isNegated = key.StartsWith("!");
      if (isNegated) key = key.Substring(1); // Remove the '!' prefix

      if (value.ValueKind == JsonValueKind.Object && value.TryGetProperty("in", out JsonElement inElement)) {
        ProcessInValue(key, inElement, criteriaList, isNegated);
      } else if (value.ValueKind == JsonValueKind.Array) {
        ProcessArrayValue(key, value, criteriaList, isNegated);
      } else {
        criteriaList.Add(new SearchCriteria(key, GetTypedItem(key, value), null, isNegated, false));
      }
    }
    return criteriaList;
  }

  private static void ProcessInValue(string key, JsonElement value, List<SearchCriteria> criteria, bool isNegated) {
    var inValues = new List<object>();
    foreach (var item in value.EnumerateArray()) {
      var newItem = GetTypedItem(key, item);
      var firstItem = inValues.Count > 0 ? inValues[0] : newItem;
      if (firstItem.GetType() != newItem.GetType())
        throw new Exception($"CriteriaConverter - Para busca IN, devem ser usados dados do mesmo tipo. {firstItem.GetType().Name} e {newItem.GetType().Name} não compativeis. Chave {key}.");
      inValues.Add(newItem);
    }
    criteria.Add(new SearchCriteria(key, inValues, null, isNegated, true));
  }

  private static void ProcessArrayValue(string key, JsonElement value, List<SearchCriteria> criteria, bool isNegated) {
    var arrayValues = new List<object>();
    foreach (var item in value.EnumerateArray()) {
      arrayValues.Add(GetTypedItem(key, item));
    }

    if (arrayValues.Count >= 2) {
      ValidateArrayValuesType(key, arrayValues);
      criteria.Add(new SearchCriteria(key, arrayValues[0], arrayValues[1], isNegated, false));
    } else if (arrayValues.Count == 1) {
      criteria.Add(new SearchCriteria(key, arrayValues[0], null, isNegated, false));
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
    if (type == null) return false;

    if (item.ValueKind == JsonValueKind.String || item.ValueKind == JsonValueKind.True || item.ValueKind == JsonValueKind.False || !item.TryGetInt32(out int idValue))
      throw new Exception($"CriteriaConverter - Para filtrar Entidades, é preciso usar valores Inteiros. Chave {key}.");

    entity = (_BaseEntityWithId)Activator.CreateInstance(type);
    entity.Id = idValue;
    return true;
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