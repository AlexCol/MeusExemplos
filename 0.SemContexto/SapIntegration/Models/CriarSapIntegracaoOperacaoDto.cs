namespace CSConnect.SapIntegration;

#region CriarSapIntegracaoOperacaoDto
public sealed class CriarSapIntegracaoOperacaoDto {
    [Required]
    [StringLength(100)]
    public required string Nome { get; init; }

    [Required]
    [StringLength(500)]
    public required string Endpoint { get; init; }

    [Required]
    [RegularExpression("^(GET|POST|PUT|PATCH|DELETE)$", ErrorMessage = "Método HTTP inválido.")]
    public required string MetodoHttp { get; init; }

    [Required]
    public required DefinicaoNoSap JsonDefinicao { get; init; }

    public bool Ativo { get; init; } = true;
}
#endregion

#region AtualizarSapIntegracaoOperacaoDto
public sealed class AtualizarSapIntegracaoOperacaoDto {
    [Required]
    [StringLength(100)]
    public required string Nome { get; init; }

    [Required]
    [StringLength(500)]
    public required string Endpoint { get; init; }

    [Required]
    [RegularExpression("^(GET|POST|PUT|PATCH|DELETE)$", ErrorMessage = "Método HTTP inválido.")]
    public required string MetodoHttp { get; init; }

    [Required]
    public required DefinicaoNoSap JsonDefinicao { get; init; }

    [Range(1, int.MaxValue)]
    public int Versao { get; init; }

    public bool Ativo { get; init; }
}
#endregion

#region RespostaSapIntegracaoOperacaoDto
public sealed class RespostaSapIntegracaoOperacaoDto {
    public long Id { get; init; }
    public string Nome { get; init; }
    public string Endpoint { get; init; }
    public string MetodoHttp { get; init; }
    public DefinicaoNoSap JsonDefinicao { get; init; }
    public int Versao { get; init; }
    public bool Ativo { get; init; }
}
#endregion
