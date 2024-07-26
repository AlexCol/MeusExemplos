using ExemploEntityFrameworkWebApi.src.extensions.toBuilder;
using ExemploEntityFrameworkWebApi.src.extensions.toApp;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();

var app = builder.Build();
app.AddDependencies();

app.Run();
