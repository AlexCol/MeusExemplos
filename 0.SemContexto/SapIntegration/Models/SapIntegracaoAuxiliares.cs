namespace CSConnect.SapIntegration;

public enum TipoNo {
    Object,
    Collection
}

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed class DefinicaoNoSap {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("type")]
    public TipoNo Tipo { get; init; } = TipoNo.Object;

    [JsonPropertyName("sql")]
    public required string Sql { get; init; }

    /*
     * Define se o nó deve obrigatoriamente retornar ao menos um registro.
     *
     * Quando não informado:
     * Object     = obrigatório
     * Collection = opcional
     *
     * O nó raiz é sempre obrigatório.
     */
    [JsonPropertyName("required")]
    public bool? Obrigatorio { get; init; }

    /*
     * Quando true, não adiciona o nó ao objeto pai se o resultado
     * for null, um objeto vazio ou uma coleção vazia.
     */
    [JsonPropertyName("omitIfEmpty")]
    public bool OmitirSeVazio { get; init; }

    /*
     * Lista os aliases SQL que devem existir e possuir valor não nulo.
     * A validação ocorre antes de aplicar Excluir.
     */
    [JsonPropertyName("requiredFields")]
    public HashSet<string> CamposObrigatorios { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    /*
     * Mapeia os parâmetros SQL para valores disponíveis no contexto.
     *
     * Exemplos:
     * "ID": "$input.ID"
     * "CHAVE1": "$parent.CHAVE1"
     */
    [JsonPropertyName("parameters")]
    public Dictionary<string, string> Parametros { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    /*
     * Colunas necessárias para os filhos, mas que não devem ser
     * incluídas no JSON enviado ao SAP.
     */
    [JsonPropertyName("exclude")]
    public HashSet<string> Excluir { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    /*
     * A chave do dicionário será o nome da propriedade no JSON.
     * O valor descreve como o nó filho será gerado.
     */
    [JsonPropertyName("children")]
    public Dictionary<string, DefinicaoNoSap> Filhos { get; init; } = new(StringComparer.Ordinal);
}

/*
 * Modelo usado apenas durante o processamento.
 * Diferentemente de SapIntegracaoPendentes, Input já está desserializado.
 */
public sealed class SapIntegracaoPendenteProcessamento {
    public long Id { get; init; }
    public long IdOperacao { get; init; }

    public Dictionary<string, object?> Input { get; init; } = new(StringComparer.OrdinalIgnoreCase);
}

/*
 * Representa dinamicamente uma linha retornada pelo banco.
 */
public sealed class DynamicRow : Dictionary<string, object?> {
    public DynamicRow() : base(StringComparer.OrdinalIgnoreCase) { }
    public DynamicRow(IDictionary<string, object?> values) : base(values, StringComparer.OrdinalIgnoreCase) { }
}
