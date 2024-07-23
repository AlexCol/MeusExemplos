using ExemploEntityFrameworkWebApi.src.extensions.toBuilder;
using ExemploEntityFrameworkWebApi.src.extensions.toApp;

var builder = WebApplication.CreateBuilder(args);
builder.addDependencies();

var app = builder.Build();
app.addDependencies();

app.Run();
