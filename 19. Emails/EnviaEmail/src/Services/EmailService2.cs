using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EnviaEmail.src.Model.Configs;
using EnviaEmail.src.Model.Email;

namespace EnviaEmail.src.Services;

public class EmailService2 {
  private EmailConfig _emailConfig;

  public EmailService2(EmailConfig emailConfig) {
    _emailConfig = emailConfig;
  }

  //! funções de preparação ------------------------------------------------------------------------------------------
  public void SendWelcomeMail(List<string> emailsTo, Attachment[] anexos = null) {
    var tilulo = "Bem vindo!";
    var corpo = "<h1>Bem vindo!!</h1>";

    var imgPath = "./documents/imageToSend.jpg";
    var view = PreparaCorpoComImagem(corpo, imgPath);

    var email = new EmailModel() {
      To = emailsTo.ConvertAll(email => new MailAddress(email)).ToArray(),
      Subject = tilulo,
      Body = corpo,
      Attachments = anexos ?? Array.Empty<Attachment>(),
      AlternatView = view
    };

    SendMail(email);
  }

  //! função de envio do email preparado ------------------------------------------------------------------------------------------
  private void SendMail(EmailModel email) {
    SmtpClient smtpClient = new SmtpClient() {
      Host = _emailConfig.SmtpServer,
      Port = _emailConfig.Port,
      EnableSsl = _emailConfig.EnableSSL,
      Credentials = new NetworkCredential(_emailConfig.EmailFrom, _emailConfig.Password),
      DeliveryMethod = SmtpDeliveryMethod.Network
    };

    MailMessage mail = new MailMessage();
    foreach (var mailTo in email.To) {
      mail.To.Add(mailTo); //pode-se enviar para mais de um destinatario
    }
    mail.From = new MailAddress(_emailConfig.EmailFrom, "Alexandre Coletti");
    mail.Subject = email.Subject;
    mail.Body = email.Body;
    mail.IsBodyHtml = true;
    foreach (var anexo in email.Attachments) {
      mail.Attachments.Add(anexo);
    }
    if (email.AlternatView != null)
      mail.AlternateViews.Add(email.AlternatView);

    smtpClient.Send(mail);

    email.AlternatViewCleanUp(); //limpeza das streams que alimentam a alternative view
    mail.Dispose();
    smtpClient.Dispose();
  }


  //! funções auxiliares ------------------------------------------------------------------------------------------
  //? prepara imagem para ser adicionada ao email
  private AlternateView PreparaCorpoComImagem(string body, string imgPath) {
    if (!File.Exists(imgPath)) throw new Exception("Arquivo não existe.");
    FileInfo file = new FileInfo(imgPath);

    LinkedResource imageResource;
    FileStream fileStream = file.OpenRead();
    imageResource = new LinkedResource(fileStream);
    imageResource.ContentId = file.Name;

    StringBuilder htmlBuilder = new StringBuilder();
    htmlBuilder.AppendLine(body);
    htmlBuilder.AppendLine("<br>");
    htmlBuilder.AppendLine($"<img src=cid:{file.Name}>");

    AlternateView html = AlternateView.CreateAlternateViewFromString(htmlBuilder.ToString(), null, "text/html");
    html.LinkedResources.Add(imageResource);
    return html;
  }


  //? prepara anexo para ser adicionada ao email
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
