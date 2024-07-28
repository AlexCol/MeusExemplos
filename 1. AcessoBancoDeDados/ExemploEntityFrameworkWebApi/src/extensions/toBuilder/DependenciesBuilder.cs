using ExemploEntityFrameworkWebApi.src.repository;
using ExemploEntityFrameworkWebApi.src.repository.Generic;
using ExemploEntityFrameworkWebApi.src.services;

namespace ExemploEntityFrameworkWebApi.src.extensions.toBuilder;

public static class DependenciesBuilder {
  public static void AddDependencies(this WebApplicationBuilder builder) {
    //!adicionando configurações padrão
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers();

    //!adicionando configurações
    builder.AddSwagger();
    builder.AddMySqlConfig();
    builder.AddLogConfig();

    //!adicionando classes para injeções de dependencia
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IPersonRepository, PersonRepository>();

    builder.Services.AddScoped<IGenderService, GenderService>();
    builder.Services.AddScoped<IPersonService, PersonService>();
    builder.Services.AddScoped<IAddressService, AddressService>();
  }
}
