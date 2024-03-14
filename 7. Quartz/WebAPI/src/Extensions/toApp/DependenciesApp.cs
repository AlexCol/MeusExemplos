namespace WebAPI.src.Extensions.toApp;

public static class DependenciesApp {
	public static void addDependencies(this WebApplication app) {
		//!adicionando configurações padrão
		app.UseCors(); //para ativar o cors
					   //app.UseCors("CORSAllowLocalHost");

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseHttpsRedirection();
		app.MapControllers();

	}
}