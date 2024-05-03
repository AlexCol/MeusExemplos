namespace Exemplo1.src.Model;

public class TokenModel {
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public int Minutes { get; set; }
    public int DaysToExpire { get; set; }
}
