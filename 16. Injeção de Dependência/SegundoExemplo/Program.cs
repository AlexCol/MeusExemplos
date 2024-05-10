using SegundoExemplo.src.Interfaces;
using SegundoExemplo.src.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddTransient<TransientService>();
builder.Services.AddSingleton<SingletonService>();
builder.Services.AddScoped<IService, ScopedService>();

var app = builder.Build();
app.MapControllers();

app.Run();
