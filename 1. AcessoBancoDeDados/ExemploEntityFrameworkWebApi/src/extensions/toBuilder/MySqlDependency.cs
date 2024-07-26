using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.extensions.toBuilder;

public static class MySqlDependency {
  public static void addMySqlConfig(this WebApplicationBuilder builder) {
    var conectionString = builder.Configuration["ConnectionStrings:MySql"];
    builder.Services.AddMySql<MyDBContext>(conectionString, ServerVersion.AutoDetect(conectionString));
  }
}
