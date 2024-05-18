using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EnviaEmail.src.Model.Email;

public class EmailModel {
  public MailAddress[] To { get; set; }
  public string Subject { get; set; }
  public string Body { get; set; }
  public Attachment[] Attachments { get; set; }
  public AlternateView AlternatView { get; set; }

  public void AlternatViewCleanUp() {
    if (AlternatView != null) { //realizando explicamente o fechamento das streams usadas para carregar as AlternateViews
      foreach (var recurso in AlternatView.LinkedResources) {
        recurso.ContentStream.Dispose();
      }
    }
  }
}
