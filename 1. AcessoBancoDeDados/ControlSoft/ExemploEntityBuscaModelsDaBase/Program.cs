using ExemploEntityBuscaModelsDaBase.src.extensions.toBuilder;
using ExemploEntityBuscaModelsDaBase.src.extensions.toApp;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();

var app = builder.Build();
app.AddDependencies();

app.Run();
