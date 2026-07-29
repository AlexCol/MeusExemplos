namespace CSConnect.SapIntegration;

[Table("TB_SAP_INTEGRACAO_PENDENTES")]
public sealed class SapIntegracaoPendentes {
    [Key]
    public long Id { get; init; }
    public long IdOperacao { get; init; }
    public string InputJson { get; init; }
    public string Status { get; init; }
    public int Tentativas { get; init; }
    public DateTime DhCriacao { get; init; }
    public DateTime? DhUltimaTentativa { get; init; }
    public DateTime? DhEnviado { get; init; }
    public string? MensagemErro { get; init; }
    public int VersaoOperacao { get; init; }
}
