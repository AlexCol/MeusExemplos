using WebApiCustomJobs.src.Extensions.toApp;
using WebApiCustomJobs.src.Extensions.toBuilder;

var builder = WebApplication.CreateBuilder(args);
builder.addDependencies();

var app = builder.Build();
app.addDependencies();

app.Run();
