namespace CSConnect.SapIntegration;

[Table("TB_SAP_INTEGRACAO_OPERACAO")]
public sealed class SapIntegracaoOperacao {
    [Key]
    public long Id { get; init; }
    public string Nome { get; init; }
    public string Endpoint { get; init; }
    public string MetodoHttp { get; init; }
    public string JsonDefinicao { get; init; }
    public int Versao { get; init; }
    public bool Ativo { get; init; }
}

