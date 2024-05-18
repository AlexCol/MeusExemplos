using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using EnviaEmail.src.Model.Configs;

namespace EnviaEmail.src.Services;

public class EmailService {
  private EmailConfig _emailConfig;

  public EmailService(EmailConfig emailConfig) {
    _emailConfig = emailConfig;
  }


  public void sendWelcomeMail(string emailTo, Attachment attachment = null) {
    var titulo = "Recuperação de senha - Projetos Alexandre";

    var body = "";
    body += "<h1>Seja bem vindo!</h1>";

    sendEmail(emailTo, titulo, body, attachment);
  }

  private void sendEmail(string emailTo, string titulo, string body, Attachment attachment) {

    var smtpClient = new SmtpClient(_emailConfig.SmtpServer) {
      Port = _emailConfig.Port,
      Credentials = new NetworkCredential(_emailConfig.EmailFrom, _emailConfig.Password),
      EnableSsl = _emailConfig.EnableSSL
    };

    var fromAddress = new MailAddress(_emailConfig.EmailFrom, "Alexandre Coletti");
    var toAddress = new MailAddress(emailTo);

    var mailMessage = new MailMessage(fromAddress, toAddress) {
      Subject = titulo,
      Body = body,
      IsBodyHtml = true
    };
    if (attachment != null)
      mailMessage.Attachments.Add(attachment);


    try {
      smtpClient.Send(mailMessage);
    } catch (Exception e) {
      throw new Exception("Erro ao enviar e-mail: " + e.Message);
    }
  }

  public Attachment PreparaAnexoPorCaminho(string filePath) {
    if (!File.Exists(filePath)) throw new Exception("Arquivo não existe.");
    FileInfo file = new FileInfo(filePath);
    return PrepareAnexo(file);
  }

  public Attachment PrepareAnexo(FileInfo file) {
    Attachment attachment = new Attachment(file.FullName, MediaTypeNames.Application.Pdf);
    attachment.Name = file.Name;
    return attachment;
  }
}
