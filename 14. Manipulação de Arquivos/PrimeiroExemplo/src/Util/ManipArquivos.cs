using System.Text;

namespace PrimeiroExemplo.src.Util;

public class ManipArquivos {
    private string _basePath = "./src/Files/";
    private string _fileName;
    private string _completeFilePath = "";

    public ManipArquivos(string fileName) {
        _fileName = fileName;
        UpdateCompletePath();
        VerificaDiretorio(_basePath);
    }

    private void UpdateCompletePath() {
        _completeFilePath = _basePath + _fileName;
    }
    private void VerificaDiretorio(string directory) {
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
    }

    public void EscreveEmArquivo(string text) {
        bool arquivoExiste = VerificaSeArquivoExiste();
        if (!arquivoExiste) {
            CriaArquivo();
        }
        using (StreamWriter writer = new StreamWriter(_completeFilePath, true)) { //! true para não sobrescrever o arquivo, realizar append, com false ele escreve por cima
            writer.Write(text);
        };
    }

    private bool VerificaSeArquivoExiste() {
        return File.Exists(_completeFilePath);
    }
    private void CriaArquivo() {
        File.Create(_completeFilePath).Close(); //pra o criar e já liberar
    }

    public string LeDeArquivo() {
        if (!VerificaSeArquivoExiste()) throw new Exception("Arquivo não existe!");

        StringBuilder builder = new StringBuilder();
        using (StreamReader reader = new StreamReader(_completeFilePath)) {
            while (!reader.EndOfStream) {
                builder.AppendLine(reader.ReadLine());
            }
        }
        return builder.ToString();
    }

    public void MoveArquivo(string destino) { //! mover = apagar da origem e recriar no destino
        VerificaDiretorio(destino);
        File.Move(_completeFilePath, destino + _fileName, true); //! com true ele apaga se o arquivo destino já existir, com false dá excessao se tiver já o arquivo
        _basePath = destino;
        UpdateCompletePath();
    }

    public void CopiaArquivo(string destino) {
        VerificaDiretorio(destino);
        File.Copy(_completeFilePath, destino + _fileName, true);
    }

    public void ExcluiArquivo() {
        if (VerificaSeArquivoExiste()) {
            File.Delete(_completeFilePath);
        }
    }
}
