using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exemplo1.src.Extensions.toBuilder;

public static class DependenciesBuilder {
    public static void addDependencies(this WebApplicationBuilder builder) {
        //!adicionando configurações padrão
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();

        //!adicionando configurações
        builder.addSwagger();
        builder.addJWTService();
    }
}