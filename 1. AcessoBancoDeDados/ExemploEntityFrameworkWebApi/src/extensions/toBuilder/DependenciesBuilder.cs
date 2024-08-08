using ExemploEntityFrameworkWebApi.src.repository;
using ExemploEntityFrameworkWebApi.src.repository.Generic;
using ExemploEntityFrameworkWebApi.src.services;
using ExemploEntityFrameworkWebApi.src.services.Generic;

namespace ExemploEntityFrameworkWebApi.src.extensions.toBuilder;

public static class DependenciesBuilder {
  public static void AddDependencies(this WebApplicationBuilder builder) {
    //!adicionando configurações padrão .Net
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers(options => {
      options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //* Remove a validação automática do ModelState (pra poder usar NotNull e não impedir o envio do json)
    });

    //!adicionando configurações via extensão
    builder.AddSwagger();
    builder.AddDBContextConfig();
    builder.AddLogConfig();

    //!adicionando classes para injeções de dependencia
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IPersonRepository, PersonRepository>();

    builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
    builder.Services.AddScoped<IGenderService, GenderService>();
    builder.Services.AddScoped<IPersonService, PersonService>();
  }
}
