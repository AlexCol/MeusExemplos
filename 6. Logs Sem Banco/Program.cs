using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console(
            Serilog.Events.LogEventLevel.Verbose,
            //"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"//como é preenchida a linha escrita do log
            "{Timestamp:dd-MM-yyyy HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        )
        .WriteTo.File(
            "./logs/meuLog.txt",
            Serilog.Events.LogEventLevel.Error,
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"//como é preenchida a linha escrita no arquivo
        );
});


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

app.MapControllers();

app.Run();
