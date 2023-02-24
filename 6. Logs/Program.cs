using Microsoft.AspNetCore.Diagnostics;
using MySql.Data.MySqlClient;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Mvc;
using Controllers;

var builder = WebApplication.CreateBuilder(args);

/* //!exemplo informado cada configuração em seu devido lugar
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console(
            restrictedToMinimumLevel: LogEventLevel.Verbose, //!descrição em 'passo_a_passo.txt'
            outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}" //!formato da string escrita {data}{nivel da mensagem}{mensagem})
        )
        .WriteTo.File(
            "./logs/meuLog_.txt" //!caminho do arquivo
            , restrictedToMinimumLevel: LogEventLevel.Warning //!descrição em 'passo_a_passo.txt'
            , outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"//!formato da string escrita {data}{nivel da mensagem}{mensagem})
            , rollingInterval: RollingInterval.Day //!de quanto em quanto tempo gera um arquivo novo (a data é informada no arquivo no formato yyyymmdd)
            , retainedFileCountLimit: 7 //!informa quantos arquivos serão mantidos no histpórico, quando um novo é criado que passe do limite, o mais antigo é eliminado
            , buffered: false
        )
        .WriteTo.MySQL(
            connectionString: context.Configuration["ConnectionStrings:WebApiDatabase"], //!string de conexão
            tableName: "TabelaLogTeste", //!tabela onde vai ser salvo os logs, se não existe será criada nova
            restrictedToMinimumLevel: LogEventLevel.Information, //!descrição em 'passo_a_passo.txt'
            storeTimestampInUtc: false, //!se false usa a hora local, se true usa a hora padrão (+3hrs)
            batchSize: 1000
        );
});
*/

//!exemplo usando dados do appsettings.json
builder.Logging.ClearProviders();
LogEventLevel logLevelAspnet = builder.Environment.IsDevelopment() ? LogEventLevel.Information : LogEventLevel.Warning;
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", logLevelAspnet)
    .CreateLogger();
builder.Host.UseSerilog(Log.Logger);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler("/error"); //mapear tratamento de erros

app.MapControllers();

app.Run();
