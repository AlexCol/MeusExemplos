using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Teste.src.Model;

namespace Teste.src.QuestPdfSpace;

public partial class QuestPdf
{
    private static void ConfigContent(PageDescriptor page, IEnumerable<Article> articles)
    {
        page.Content()
            .PaddingVertical(1, Unit.Centimetre)
            .Column(x =>
            {
                x.Spacing(20);

                x.Item().Text(Placeholders.LoremIpsum());

                //x.Item().Image(Placeholders.Image(200, 100));
                x.Item().Container()
                    .Width(200)   // Define a largura do container
                    .Height(150)  // Define a altura do container
                    .Image("c:\\Users\\A1988\\OneDrive\\Área de Trabalho\\ControLogo.png");

                x.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50);
                        columns.RelativeColumn(2);
                        columns.ConstantColumn(60);
                        columns.RelativeColumn(1);
                    });

                    table.Cell().Row(1).Column(1).Element(Block).Text("Article ID").FontSize(12).SemiBold();
                    table.Cell().Row(1).Column(2).Element(Block).AlignLeft().PaddingLeft(6).Text("Product Name").FontSize(12).SemiBold();
                    table.Cell().Row(1).Column(3).Element(Block).Text("Stock").FontSize(12).SemiBold();
                    table.Cell().Row(1).Column(4).Element(Block).Text("Price").FontSize(12).SemiBold();

                    uint rowIndex = 2;
                    foreach (var article in articles)
                    {
                        table.Cell().Row(rowIndex).Column(1).Element(Entry).Text(article.ArticleId.ToString());
                        table.Cell().Row(rowIndex).Column(2).Element(Entry).AlignLeft().Text(article.ProductName);
                        table.Cell().Row(rowIndex).Column(3).Element(Entry).Text(article.Stock.ToString());
                        table.Cell().Row(rowIndex).Column(4).Element(Entry).Text(article.Price.ToString("C"));
                        rowIndex++;
                    }

                    // Cabeçalho da tabela
                    // table.Header(header =>
                    // {
                    //     header.Cell().Text("Article ID").FontSize(12).Bold();
                    //     header.Cell().Text("Product Name").FontSize(12).Bold();
                    //     header.Cell().Text("Stock").FontSize(12).Bold();
                    //     header.Cell().Text("Price").FontSize(12).Bold();
                    // });

                    // // Iterar sobre os artigos e adicionar os dados às células
                    // foreach (var article in articles)
                    // {
                    //     table.Cell().Text(article.ArticleId.ToString());
                    //     table.Cell().Text(article.ProductName);
                    //     table.Cell().Text(article.Stock.ToString());
                    //     table.Cell().Text(article.Price.ToString("C")); // Formatar como valor monetário
                    // }
                });
            });
    }

    static IContainer Block(this IContainer container)
    {
        return container
            .Border(1)
            .Background(Colors.Grey.Lighten1)
            .ShowOnce()
            .MinWidth(50)
            .MinHeight(50)
            .AlignCenter()
            .AlignMiddle();
    }

    static IContainer Entry(this IContainer container)
    {
        return container
            .Border(1)
            .PaddingTop(1)
            .PaddingBottom(1)
            .PaddingLeft(6)
            .PaddingRight(6)
            .ShowOnce()
            .AlignCenter()
            .AlignMiddle();
    }
}
