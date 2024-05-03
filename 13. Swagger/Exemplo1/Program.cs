using Exemplo1.src.Extensions.toApp;
using Exemplo1.src.Extensions.toBuilder;

var builder = WebApplication.CreateBuilder(args);
builder.addDependencies();

var app = builder.Build();
app.addDependencies();
app.Run();

