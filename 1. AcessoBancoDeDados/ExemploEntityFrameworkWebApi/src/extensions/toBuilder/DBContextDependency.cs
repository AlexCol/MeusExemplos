using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.extensions.toBuilder;

public static class DBContextDependency {
  public static void AddDBContextConfig(this WebApplicationBuilder builder) {
    var bancoAlvo = builder.Configuration["BancoAlvo"].ToLower();
    switch (bancoAlvo) {
      case "mysql":
        AddMySqlConfig(builder);
        break;
      case "firebird":
        AddFirebirdConfig(builder);
        break;
      default:
        throw new Exception("Não definido banco de dados alvo.");
    }
  }

  private static void AddMySqlConfig(WebApplicationBuilder builder) {
    var conectionString = builder.Configuration["ConnectionStrings:MySql"];
    builder.Services.AddMySql<MyDBContext>(conectionString, ServerVersion.AutoDetect(conectionString));
  }

  private static void AddFirebirdConfig(WebApplicationBuilder builder) {
    var conectionString = builder.Configuration["ConnectionStrings:Firebird"];
    builder.Services.AddFirebird<MyDBContext>(conectionString);
  }
}
