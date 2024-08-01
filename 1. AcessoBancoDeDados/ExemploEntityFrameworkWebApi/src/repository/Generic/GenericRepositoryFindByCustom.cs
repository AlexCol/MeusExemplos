using System.Reflection;
using System.Linq.Expressions;
using ExemploEntityFrameworkWebApi.src.models;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public partial class GenericRepository<T> : IGenericRepository<T> where T : _BaseEntityWithId {
  private async Task<List<T>> FindByPropertiesAsync(Dictionary<string, object> properties) {
    var propertyTuples = properties.Select(p => Tuple.Create(p.Key, p.Value, (object)null, false)).ToList();
    return await FindByPropertiesAsync(propertyTuples);
  }

  private async Task<List<T>> FindByPropertiesAsync(List<Tuple<string, object, object, bool>> properties) {
    IQueryable<T> query = PrepareQuery();

    foreach (var property in properties) {
      var predicate = CreatePredicate(property);
      if (predicate != null) {
        query = query.Where(predicate);
      }
    }

    return await query.ToListAsync();
  }

  private Expression<Func<T, bool>> CreatePredicate(Tuple<string, object, object, bool> property) {
    var parameter = Expression.Parameter(typeof(T), "x");
    var member = Expression.Property(parameter, property.Item1);

    Expression body;
    if (property.Item2 != null && property.Item3 != null) {
      body = CreateBetweenExpression(member, property.Item2, property.Item3);
    } else if (property.Item2 is string stringValue) {
      body = CreateStringContainsExpression(member, stringValue);
    } else if (property.Item2 is _BaseEntityWithId entityValue) {
      body = CreateEntityIdEqualsExpression(member, entityValue);
    } else {
      body = CreateEqualsExpression(member, property.Item2);
    }

    if (property.Item4) {
      body = Expression.Not(body);
    }

    return body != null ? Expression.Lambda<Func<T, bool>>(body, parameter) : null;
  }

  private Expression CreateBetweenExpression(MemberExpression member, object startValue, object endValue) {
    if (startValue is DateTime startDate && endValue is DateTime endDate) {
      return CreateBetweenDatesExpression(member, startDate, endDate);
    } else if (IsNumericType(startValue) && IsNumericType(endValue)) {
      return CreateBetweenNumericExpression(member, startValue, endValue);
    } else if (startValue is _BaseEntityWithId startEntity && endValue is _BaseEntityWithId endEntity) {
      return CreateBetweenEntityIdExpression(member, startEntity, endEntity);
    } else {
      throw new Exception("GenericRepositoryFindByCustom - Informado dois valores que não são compativeis para buscar valores 'entre'. Deve ser data ou numero ou uma Entidade.");
    }
  }

  private Expression CreateBetweenDatesExpression(MemberExpression member, DateTime startDate, DateTime endDate) {
    var startConstant = Expression.Constant(startDate);
    var endConstant = Expression.Constant(endDate);
    var greaterThanOrEqual = Expression.GreaterThanOrEqual(member, startConstant);
    var lessThanOrEqual = Expression.LessThanOrEqual(member, endConstant);
    return Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);
  }

  private Expression CreateBetweenNumericExpression(MemberExpression member, object startValue, object endValue) {
    var startConstant = Expression.Constant(startValue);
    var endConstant = Expression.Constant(endValue);
    var greaterThanOrEqual = Expression.GreaterThanOrEqual(member, startConstant);
    var lessThanOrEqual = Expression.LessThanOrEqual(member, endConstant);
    return Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);
  }

  private Expression CreateBetweenEntityIdExpression(MemberExpression member, _BaseEntityWithId startEntity, _BaseEntityWithId endEntity) {
    var idMember = Expression.Property(member, "Id");
    var startConstant = Expression.Constant(startEntity.Id);
    var endConstant = Expression.Constant(endEntity.Id);
    var greaterThanOrEqual = Expression.GreaterThanOrEqual(idMember, startConstant);
    var lessThanOrEqual = Expression.LessThanOrEqual(idMember, endConstant);
    return Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);
  }

  private Expression CreateStringContainsExpression(MemberExpression member, string stringValue) {
    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
    var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);

    var memberToLower = Expression.Call(member, toLowerMethod);
    var constantToLower = Expression.Constant(stringValue.ToLower());

    return Expression.Call(memberToLower, containsMethod, constantToLower);
  }

  private Expression CreateEntityIdEqualsExpression(MemberExpression member, _BaseEntityWithId entityValue) {
    var idMember = Expression.Property(member, "Id");
    var constant = Expression.Constant(entityValue.Id);
    return Expression.Equal(idMember, constant);
  }

  private Expression CreateEqualsExpression(MemberExpression member, object value) {
    var constant = Expression.Constant(value);
    return Expression.Equal(member, constant);
  }

  private bool IsNumericType(object value) {
    return value is sbyte || value is byte || value is short || value is ushort ||
           value is int || value is uint || value is long || value is ulong ||
           value is float || value is double || value is decimal;
  }
}
