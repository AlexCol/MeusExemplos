namespace WebAPI.src.Extensions.toBuilder;

public static class DependenciesBuilder {
	public static void addDependencies(this WebApplicationBuilder builder) {
		//!adicionando configurações padrão
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddControllers();

		//!adicionando configurações
		builder.addLogService();
		builder.AddQuartz();
		//!adicionando classes para injeções de dependencia

	}
}