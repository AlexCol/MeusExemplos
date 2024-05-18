using System.Net.Mail;
using EnviaEmail.src.Services;

namespace EnviaEmail.src.Testes;

public static class TestaEmailService2 {
  public static void Run() {
    var emailConfig = Configurator.GetEmailConfig();
    var mailService = new EmailService2(emailConfig);

    Console.WriteLine("Informe o email pra quem deseja enviar:");
    var email = Console.ReadLine();
    //mailService.SendWelcomeMail([email]); //sem anexo


    Attachment[] anexos = PreparaAnexosMain(mailService);
    List<string> emailsDestino = [email, "hagodax686@qiradio.com"];
    mailService.SendWelcomeMail(emailsDestino, anexos.ToArray());
  }


  private static Attachment[] PreparaAnexosMain(EmailService2 mailService) {
    List<Attachment> anexos = new List<Attachment>();
    var pdfPath = "./documents/toSend.pdf";
    anexos.Add(mailService.PreparaAnexoPorCaminho(pdfPath));
    var pdfPath2 = "./documents/bloqs.pdf";
    anexos.Add(mailService.PreparaAnexoPorCaminho(pdfPath2));

    return anexos.ToArray();
  }
}
