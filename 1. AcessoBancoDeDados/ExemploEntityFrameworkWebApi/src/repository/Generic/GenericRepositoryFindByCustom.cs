// GenericRepository.cs
using System.Linq.Expressions;
using ExemploEntityFrameworkWebApi.src.models;
using Microsoft.EntityFrameworkCore;
using ExemploEntityFrameworkWebApi.src.models.search;

namespace ExemploEntityFrameworkWebApi.src.repository.Generic;

public partial class GenericRepository<T> : IGenericRepository<T> where T : _BaseEntityWithId {
  /*
    ! O método FindByPropertiesAsync é usado realizar buscas usando uma lista de SearchCriteria.
    ! Com ele é possível realizar buscas com base no nome da Propriedade da classe T.
    ! Atualmente é comportado:
    ! Busca direta: "FirstName": "ale";
    ! Busca com valor Between (numeros, data e entidades): "Gender": [1, 2] ou "DateOfBirth": ["26-06-1978", "20-12-1979"]
    ! Busca maior ou igual a: "Id": [4, "+"] (no primeiro valor usar o valor desejado - id de entidade, numero ou data - com o segundo valor do array com um '+')
    ! Busca menor ou igual a: "Id": ["-", 3] (colocar um '-' no primeiro valor e usar o valor desejado - id de entidade, numero ou data - no segundo do array)
    ! Negação: Qualquer campo que tenha em seu nome iniciando com '!' será com Not. ex. "!LastName": "coletti" para não vir registros com LastName que tenham 'coletti';
  */
  private async Task<List<T>> FindByPropertiesAsync(List<SearchCriteria> criteriaList) {
    IQueryable<T> query = PrepareQuery();

    foreach (var criteria in criteriaList) {
      var predicate = CreatePredicate(criteria);
      if (predicate != null) {
        query = query.Where(predicate);
      }
    }

    return await query.ToListAsync();
  }

  private Expression<Func<T, bool>> CreatePredicate(SearchCriteria criteria) {
    var parameter = Expression.Parameter(typeof(T), "x");
    var member = Expression.Property(parameter, criteria.Key);

    Expression body;
    if (criteria.IsIn) {
      body = CreateInExpression(member, (List<object>)criteria.Value1);
    } else if (criteria.Value1 != null && criteria.Value2 != null) {
      if (criteria.Value1.GetType() == typeof(string) && criteria.Value1.ToString() == "-") {
        body = CreateLessThanOrEqualExpression(member, criteria.Value2);
      } else if (criteria.Value2.GetType() == typeof(string) && criteria.Value2.ToString() == "+") {
        body = CreateGreaterThanOrEqualExpression(member, criteria.Value1);
      } else {
        body = CreateBetweenExpression(member, criteria.Value1, criteria.Value2);
      }
    } else if (criteria.Value1 is string stringValue) {
      body = CreateStringContainsExpression(member, stringValue);
    } else if (criteria.Value1 is _BaseEntityWithId entityValue) {
      body = CreateEntityIdEqualsExpression(member, entityValue);
    } else {
      body = CreateEqualsExpression(member, criteria.Value1);
    }

    if (criteria.IsNegated) {
      body = Expression.Not(body);
    }

    return body != null ? Expression.Lambda<Func<T, bool>>(body, parameter) : null;
  }

  private Expression CreateLessThanOrEqualExpression(MemberExpression member, object value) {
    return CreateComparisonExpression(member, value, Expression.LessThanOrEqual);
  }

  private Expression CreateGreaterThanOrEqualExpression(MemberExpression member, object value) {
    return CreateComparisonExpression(member, value, Expression.GreaterThanOrEqual);
  }

  private Expression CreateComparisonExpression(MemberExpression member, object value, Func<Expression, Expression, BinaryExpression> comparison) {
    if (value is not (DateTime or _BaseEntityWithId) && !IsNumericType(value)) {
      throw new ArgumentException("GenericRepositoryFindByCustom - Para buscas 'menor ou maior a' é preciso que o dado seja  data ou numero ou uma Entidade.");
    }
    var constant = Expression.Constant(value);
    return comparison(member, constant);
  }

  private Expression CreateInExpression(MemberExpression member, List<object> values) {
    var expressions = values.Select(value => Expression.Equal(member, Expression.Constant(value)));
    var combined = expressions.Aggregate(Expression.OrElse);
    return combined;
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
    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

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
