
using Api.src.services.httpServices;
using Serilog;

namespace Api.src.extensions.toBuilder;

public static class DependenciesBuilder {
  public static void AddDependencies(this WebApplicationBuilder builder) {
    //!adicionando configurações padrão .Net
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers();

    //!adicionando configurações via extensão
    builder.AddSwagger();
    builder.AddLogConfig();
    builder.AddPolly();

    //!adicionando classes para injeções de dependencia
    //builder.Services.AddTransient<ITestApiRequests, TestApiRequests>(); //como é usada na polly, não é preciso injetar ela diretamente, ou se o fizer, ela deve estar acima da poli
  }
}
