using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CriandoScaffoldComConsole.src.Util;

public class ConfigData {
  private readonly IConfiguration _config;
  public ConfigData() {
    _config = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .Build();
  }
  public IConfiguration GetConfig() {
    return _config;
  }
}