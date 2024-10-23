using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using Teste.src.Model;
using Teste.src.QuestPdfSpace;

namespace Teste.src.Services;

public interface IPrintService {
    Task PrintFile(IFormFile file, string printerName);
    void PrintGeneratedPdf(IEnumerable<Article> articles, string printerName);
}

public class PrintService : IPrintService {
    public async Task PrintFile(IFormFile file, string printerName) {
        if (file == null || string.IsNullOrEmpty(printerName)) {
            throw new Exception("Arquivo e nome da impressora são obrigatórios.");
        }

        // Verifica se a impressora existe
        PrinterSettings printerSettings = new PrinterSettings();
        if (!printerSettings.IsValid || printerSettings.PrinterName != printerName) {
            throw new Exception("Impressora não encontrada.");
        }

        // Extrair a extensão original do arquivo
        string fileExtension = Path.GetExtension(file.FileName).ToLower();
        // Gerar um caminho temporário com a mesma extensão do arquivo original
        string tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{fileExtension}");

        // Salvar o arquivo temporariamente com a extensão correta
        using (var stream = new FileStream(tempFilePath, FileMode.Create)) {
            await file.CopyToAsync(stream);
        }

        try {
            switch (fileExtension) {
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".bmp":
                    PrintImage(tempFilePath, printerName);
                    break;

                case ".pdf":
                    PrintPdf(tempFilePath, printerName);
                    break;

                case ".txt":
                    PrintTextFile(tempFilePath, printerName);
                    break;

                case ".docx":
                    PrintWordDocument(tempFilePath, printerName);
                    break;

                case ".emf":
                    PrintMetaFile(tempFilePath, printerName);
                    break;

                default:
                    throw new Exception("Tipo de arquivo não suportado para impressão.");
            }

            System.IO.File.Delete(tempFilePath); // Limpar o arquivo temporário
        } catch (Exception ex) {
            throw new Exception($"Erro ao tentar imprimir: {ex.Message}");
        }
    }

    public void PrintGeneratedPdf(IEnumerable<Article> articles, string printerName) {
        string pdfFilePath = QuestPdf.Init(articles, true);

        PrintPdf(pdfFilePath, printerName);

        if (File.Exists(pdfFilePath))
            File.Delete(pdfFilePath);
    }

    private void PrintMetaFile(string tempFilePath, string printerName) {
        Metafile metafile = new Metafile(tempFilePath);
        PrintDocument printDocument = new PrintDocument();
        printDocument.PrinterSettings.PrinterName = printerName;
        printDocument.PrintPage += (sender, e) => {
            RectangleF printArea = e.PageBounds;
            e.Graphics.DrawImage(metafile, printArea);
        };

        printDocument.Print();
        metafile.Dispose();
    }

    // Método para imprimir imagens
    private void PrintImage(string filePath, string printerName) {
        Image image = Image.FromFile(filePath);
        PrintDocument printDocument = new PrintDocument();
        printDocument.PrinterSettings.PrinterName = printerName;

        printDocument.PrintPage += (sender, e) => {
            RectangleF printArea = e.PageBounds;
            e.Graphics.DrawImage(image, printArea);
        };

        printDocument.Print();
        image.Dispose();
    }

    // Método para imprimir arquivos de texto
    private void PrintTextFile(string filePath, string printerName) {
        string fileContent = System.IO.File.ReadAllText(filePath);
        PrintDocument printDocument = new PrintDocument();
        printDocument.PrinterSettings.PrinterName = printerName;

        printDocument.PrintPage += (sender, e) => {
            e.Graphics.DrawString(fileContent, new Font("Arial", 12), System.Drawing.Brushes.Black, new RectangleF(0, 0, e.PageBounds.Width, e.PageBounds.Height));
        };

        printDocument.Print();
    }

    // Método para imprimir PDFs (usa SumatraPDF)
    private void PrintPdf(string filePath, string printerName) {
        // Caminho para o executável do SumatraPDF
        string sumatraPdfPath = @"C:\Program Files\SumatraPDF\SumatraPDF.exe"; // Ajuste o caminho conforme necessário

        // Argumentos para impressão
        string arguments = $"-print-to \"{printerName}\" \"{filePath}\"";

        // Criação do processo para chamar o SumatraPDF
        var processStartInfo = new ProcessStartInfo {
            FileName = sumatraPdfPath,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(processStartInfo);
        process.WaitForExit();
    }

    // Método para imprimir documentos Word (usa OpenXML ou Aspose.Words)
    private void PrintWordDocument(string filePath, string printerName) {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false)) {
            // Extrair o texto do documento Word
            string text = wordDoc.MainDocumentPart.Document.Body.InnerText;

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = printerName;

            printDocument.PrintPage += (sender, e) => {
                e.Graphics.DrawString(text, new Font("Arial", 12), System.Drawing.Brushes.Black, new RectangleF(0, 0, e.PageBounds.Width, e.PageBounds.Height));
            };

            printDocument.Print();
        }
    }
}
