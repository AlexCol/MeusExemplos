using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExemploEntityFrameworkWebApi.src.models.contexts;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityFrameworkWebApi.src.extensions.toBuilder;

public static class MySqlDependency {
  public static void addMySqlConfig(this WebApplicationBuilder builder) {
    var conectionString = builder.Configuration["ConnectionStrings:MySql"];
    builder.Services.AddMySql<MySqlContext>(conectionString, ServerVersion.AutoDetect(conectionString));
  }
}
