using QuestPDF.Infrastructure;
using Teste.src.Model;
using Teste.src.QuestPdfSpace;
using Teste.src.Services;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IPrintService, PrintService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

// var articles = ArticleData.getMockArticles();
// QuestPdf.Init(articles, false);

app.Run();

