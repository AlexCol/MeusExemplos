using System.Drawing;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Teste.src.Model;
using Teste.src.Services;

namespace Teste.src.QuestPdfSpace;

public static partial class QuestPdf {
    public static string Init(IEnumerable<Article> article, bool print = false) {

        QuestPDF.Settings.License = LicenseType.Community;
        var document = Document.Create(container => {

            container.Page(page => {
                ConfigGeral(page);
                ConfigHeader(page);
                ConfigContent(page, article);
                ConfigFooter(page);
            });
        });

        //! pra mostrar o documento
        //

        if (!print) {
            document.ShowInCompanionAsync(12500);
            return string.Empty;
        }

        string tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
        document.GeneratePdf(tempFilePath);
        return tempFilePath;
    }
}
