using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnviaEmail.src.Model.Configs;

public class EmailConfig {
  public string SmtpServer { get; set; }
  public int Port { get; set; }
  public bool EnableSSL { get; set; }
  public string EmailFrom { get; set; }
  public string Password { get; set; }
}
