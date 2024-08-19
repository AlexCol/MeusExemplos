using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers_Genericos.src.Extensions.ToApp;

public static class AppDependencies {
  public static void AddDependencies(this WebApplication app) {
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment()) {
      app.UseSwagger();
      app.UseSwaggerUI();
    }
    app.UseRouting();
    app.MapControllers();
    // app.UseEndpoints(endpoints => {
    //   endpoints.MapControllers();
    // });

    app.UseHttpsRedirection();
  }
}
