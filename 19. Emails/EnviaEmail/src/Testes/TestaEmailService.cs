using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnviaEmail.src.Services;

namespace EnviaEmail.src.Testes;

public static class TestaEmailService {
  public static void Run() {
    var emailConfig = Configurator.GetEmailConfig();
    var mailService = new EmailService(emailConfig);

    Console.WriteLine("Informe o email pra quem deseja enviar:");
    var email = Console.ReadLine();
    mailService.sendWelcomeMail(email); //sem anexo


    var pdfPath = "./documents/toSend.pdf";
    var attachment = mailService.PreparaAnexoPorCaminho(pdfPath);
    mailService.sendWelcomeMail(email, attachment);
  }
}
