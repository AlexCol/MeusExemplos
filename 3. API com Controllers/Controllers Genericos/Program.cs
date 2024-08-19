using Controllers_Genericos.src.Extensions.ToApp;
using Controllers_Genericos.src.Extensions.ToBuilder;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();

var app = builder.Build();
app.AddDependencies();
app.Run();