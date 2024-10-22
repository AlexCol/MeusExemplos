using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Teste.src.QuestPdfSpace;

public partial class QuestPdf
{
    private static void ConfigHeader(PageDescriptor page)
    {
        page.Header()
            .Text(txt =>
            {
                txt.Span("Cabeçalho")
                    .Style(new TextStyle()
                        .Bold()
                        .FontColor(Colors.Purple.Darken1)
                        .FontSize(30));

                txt.EmptyLine();
                txt.Span(" do documento")
                    .Style(new TextStyle()
                        .Italic()
                        .FontColor(Colors.Purple.Darken1)
                        .FontSize(20));
            });
    }
}
