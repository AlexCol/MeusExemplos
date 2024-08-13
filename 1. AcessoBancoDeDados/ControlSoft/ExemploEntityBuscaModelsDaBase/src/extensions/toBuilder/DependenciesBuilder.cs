namespace ExemploEntityBuscaModelsDaBase.src.extensions.toBuilder;

public static class DependenciesBuilder {
  public static void AddDependencies(this WebApplicationBuilder builder) {
    //!adicionando configurações padrão .Net
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers(options => {
      options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //* Remove a validação automática do ModelState (pra poder usar NotNull e não impedir o envio do json)
    });

    //!adicionando configurações via extensão
    builder.AddSwagger();
    //builder.AddDBContextConfig();
    builder.AddLogConfig();

    //!adicionando classes para injeções de dependencia

  }
}
