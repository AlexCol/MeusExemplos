using System.Reflection;
using InjeçãoDinamica.Properties.src.config.DI.extension;
using InjeçãoDinamica.Properties.src.services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAutoInjectables(Assembly.GetExecutingAssembly());

var app = builder.Build();
app.MapControllers();

app.Run();
