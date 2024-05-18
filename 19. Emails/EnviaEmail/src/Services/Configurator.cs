using EnviaEmail.src.Model.Configs;
using Microsoft.Extensions.Configuration;

namespace EnviaEmail.src.Services;

public static class Configurator {
  private static IConfigurationRoot GetConfiguracao() {
    IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    return configuration;
  }

  public static EmailConfig GetEmailConfig() {
    var config = GetConfiguracao();
    EmailConfig emailConfig = new EmailConfig() {
      SmtpServer = config["Email:smtpServer"],
      Port = int.Parse(config["Email:port"]),
      EnableSSL = bool.Parse(config["Email:enableSSL"]),
      EmailFrom = config["Email:emailFrom"],
      Password = config["Email:password"]
    };

    return emailConfig;
  }


}
