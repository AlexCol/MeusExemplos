using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Teste.src.QuestPdfSpace;

public partial class QuestPdf {
    private static void ConfigFooter(PageDescriptor page) {
        TextStyle style1 = new TextStyle()
            .FontColor(Colors.Red.Darken1)
            .Underline()
            .Bold();

        page.Footer()
            //.Padding(1, Unit.Centimetre)
            .BorderLeft(1, Unit.Millimetre)
            .BorderTop(1, Unit.Millimetre)
            .BorderRight(1, Unit.Millimetre)
            .BorderBottom(1, Unit.Millimetre)
            .BorderColor(Colors.Cyan.Lighten1)
            .Background(Colors.Yellow.Lighten4)
            .AlignCenter()
            .Text(txt => {
                txt.Span("Page ").Style(style1);
                txt.CurrentPageNumber().Underline().FontColor(Colors.Blue.Accent4);
            });
    }
}
