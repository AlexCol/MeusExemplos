using Microsoft.EntityFrameworkCore;

namespace ExemploEntityBuscaModelsDaBase.src.extensions.toBuilder;

public static class DBContextDependency {
  public static void AddDBContextConfig(this WebApplicationBuilder builder) {
    // var bancoAlvo = builder.Configuration["BancoAlvo"].ToLower();
    // switch (bancoAlvo) {
    //   case "mysql":
    //     AddMySqlConfig(builder);
    //     break;
    //   case "firebird":
    //     AddFirebirdConfig(builder);
    //     break;
    //   default:
    //     throw new Exception("Não definido banco de dados alvo.");
    // }
  }

  // private static void AddFirebirdConfig(WebApplicationBuilder builder) {
  //   var conectionString = builder.Configuration["ConnectionStrings:Firebird"];
  //   builder.Services.AddFirebird<MyDBContext>(conectionString);
  // }
}
