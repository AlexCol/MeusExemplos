using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exemplo1.src.Extensions.toApp;

public static class DependenciesApp {
    public static void addDependencies(this WebApplication app) {
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{"Rest Api para testar Swagger"}"));

        app.UseHttpsRedirection();
        app.MapControllers();
    }
}
