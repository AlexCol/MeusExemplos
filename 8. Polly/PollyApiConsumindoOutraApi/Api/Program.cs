using Api.src.extensions.toApp;
using Api.src.extensions.toBuilder;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();

var app = builder.Build();
app.AddDependencies();
app.Run();