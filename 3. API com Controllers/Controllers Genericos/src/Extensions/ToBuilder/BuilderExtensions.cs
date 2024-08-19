using Controllers_Genericos.src.Controllers.GenericControllers.Util;
using Controllers_Genericos.src.Repository;

namespace Controllers_Genericos.src.Extensions.ToBuilder;

public static class BuilderExtensions {
  public static void AddDependencies(this WebApplicationBuilder builder) {
    //! default dependencies
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddControllersWithViews(options => {
      options.Conventions.Add(new ControllerGenericoCrudRouteConvention());
    })
    .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new ControllerGenericoCrudTypeFeatureProvider()));

    //!Add dependencies
    builder.Services.AddSingleton(typeof(IRepositoryGenericoCrud<>), typeof(RepositoryGenericoCrud<>)); //?feito como singleton para manter a lista na memoria
  }
}
