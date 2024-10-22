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
    private static void ConfigGeral(PageDescriptor page)
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.Green.Lighten4);
        page.DefaultTextStyle(style =>
        {
            style = style.FontSize(12);
            style = style.FontColor(Colors.Blue.Darken1);
            return style;
        });
    }
}
