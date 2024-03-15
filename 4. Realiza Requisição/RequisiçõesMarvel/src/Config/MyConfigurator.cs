using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RequisiçõesMarvel.src.Config;

public static class MyConfigurator {
  private static DateTime _ts;
  private static IConfiguration Config { get; set; } = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json")
      .Build();

  public static DateTime TS {
    get {
      return _ts;
    }
    set {
      _ts = value;
    }
  }

  public static string getPublicKey() {
    return Config["MarvelKeys:PublicKey"];
  }

  public static string getHash() {
    var tsForHash = TS.ToString("yyyy-dd-MMHH:mm:ss:ffff", CultureInfo.InvariantCulture);
    var publicKey = Config["MarvelKeys:PublicKey"];
    var privateKey = Config["MarvelKeys:PrivateKey"];

    MD5 md5Hash = MD5.Create();
    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(tsForHash + privateKey + publicKey));
    StringBuilder sBuilder = new StringBuilder();
    for (int i = 0; i < data.Length; i++) {
      sBuilder.Append(data[i].ToString("x2"));
    }
    return sBuilder.ToString();
  }
}
