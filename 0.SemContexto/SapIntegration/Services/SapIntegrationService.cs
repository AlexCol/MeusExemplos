using System.Text.Json.Nodes;

namespace CSConnect.SapIntegration;

public interface ISapIntegracaoService {
    Task ProcessarIntegracoesAsync();
    Task<JsonNode> GerarPayloadAsync(long idOperacao, IReadOnlyDictionary<string, JsonElement> inputJson);
    Task<IEnumerable<RespostaSapIntegracaoOperacaoDto>> ObterOperacoesAsync();
    Task<RespostaSapIntegracaoOperacaoDto?> ObterOperacaoPorIdAsync(long id);
    Task<RespostaSapIntegracaoOperacaoDto> CriarOperacaoAsync(CriarSapIntegracaoOperacaoDto dto);
    Task<RespostaSapIntegracaoOperacaoDto> AtualizarOperacaoAsync(long id, AtualizarSapIntegracaoOperacaoDto dto);
    Task DesativarOperacaoAsync(long id);
}

public class SapIntegracaoService : ISapIntegracaoService {
    /**********************************************/
    /* Propriedades                               */
    /**********************************************/
    #region Propriedades
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    private static readonly JsonSerializerOptions JsonOptions = new() {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };
    private readonly ISapIntegracaoRepository _sapIntegracaoRepository;
    private readonly HttpClient _httpClient;
    #endregion

    /**********************************************/
    /* Construtor e Métodos Públicos              */
    /**********************************************/
    #region Construtor e Métodos Públicos
    public SapIntegracaoService(ISapIntegracaoRepository sapIntegracaoRepository, HttpClient httpClient) {
        _sapIntegracaoRepository = sapIntegracaoRepository;
        _httpClient = httpClient;
    }

    public async Task ProcessarIntegracoesAsync() {
        await Semaphore.WaitAsync();

        try {
            var operacoes = await _sapIntegracaoRepository.ObterOperacoesAtivasAsync();
            if (!operacoes.Any())
                return;

            // await RealizaLogin(); //! aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            foreach (var operacao in operacoes) {
                await ProcessarIntegracaoAsync(operacao);
            }
        } finally {
            // await RealizaLogout(); //! aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

            Semaphore.Release();
        }
    }

    public async Task<JsonNode> GerarPayloadAsync(long idOperacao, IReadOnlyDictionary<string, JsonElement> inputJson) {
        if (inputJson.Count == 0)
            throw new InvalidOperationException("O INPUTJSON deve possuir ao menos um parâmetro.");

        var operacao = await _sapIntegracaoRepository.ObterOperacaoPorIdAsync(idOperacao)
            ?? throw new KeyNotFoundException($"Operação com ID '{idOperacao}' não encontrada.");

        var definicao = DesserializarDefinicao(operacao.JsonDefinicao);
        ValidaDefinicao(definicao);

        var input = ConverteInputJson(inputJson);
        return await ProcessaNo(definicao, input, parent: null, caminhoNo: "$root")
            ?? throw new InvalidOperationException("O nó raiz não gerou conteúdo.");
    }

    public async Task<IEnumerable<RespostaSapIntegracaoOperacaoDto>> ObterOperacoesAsync() {
        var operacoes = await _sapIntegracaoRepository.ObterTodasOperacoesAsync();
        return operacoes.Select(operacao => new RespostaSapIntegracaoOperacaoDto {
            Id = operacao.Id,
            Nome = operacao.Nome,
            Endpoint = operacao.Endpoint,
            MetodoHttp = operacao.MetodoHttp,
            JsonDefinicao = DesserializarDefinicao(operacao.JsonDefinicao),
            Versao = operacao.Versao,
            Ativo = operacao.Ativo
        });
    }

    public async Task<RespostaSapIntegracaoOperacaoDto?> ObterOperacaoPorIdAsync(long id) {
        var operacao = await _sapIntegracaoRepository.ObterOperacaoPorIdAsync(id);
        var response = new RespostaSapIntegracaoOperacaoDto {
            Id = operacao?.Id ?? 0,
            Nome = operacao?.Nome ?? string.Empty,
            Endpoint = operacao?.Endpoint ?? string.Empty,
            MetodoHttp = operacao?.MetodoHttp ?? string.Empty,
            JsonDefinicao = DesserializarDefinicao(operacao?.JsonDefinicao ?? "{}"),
            Versao = operacao?.Versao ?? 0,
            Ativo = operacao?.Ativo ?? false
        };
        return response;
    }

    public async Task<RespostaSapIntegracaoOperacaoDto> CriarOperacaoAsync(CriarSapIntegracaoOperacaoDto dto) {
        ValidaDefinicao(dto.JsonDefinicao);

        var jsonDefinicao = JsonSerializer.Serialize(dto.JsonDefinicao, JsonOptions);

        var id = await _sapIntegracaoRepository.CriarOperacaoAsync(dto, jsonDefinicao);

        return await ObterOperacaoPorIdAsync(id)
            ?? throw new InvalidOperationException($"A operação criada com ID '{id}' não foi encontrada.");
    }

    public async Task<RespostaSapIntegracaoOperacaoDto> AtualizarOperacaoAsync(long id, AtualizarSapIntegracaoOperacaoDto dto) {
        ValidaDefinicao(dto.JsonDefinicao);

        var operacaoAtual = await _sapIntegracaoRepository.ObterOperacaoPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Operação com ID '{id}' não encontrada.");

        if (operacaoAtual.Versao != dto.Versao) {
            var msgErro = $"A operação foi alterada por outro processo. Versão informada: {dto.Versao}; versão atual: {operacaoAtual.Versao}.";
            throw new InvalidOperationException(msgErro);
        }

        var jsonDefinicao = JsonSerializer.Serialize(dto.JsonDefinicao, JsonOptions);

        await _sapIntegracaoRepository.AtualizarOperacaoAsync(id, dto, jsonDefinicao);

        return await ObterOperacaoPorIdAsync(id)
            ?? throw new InvalidOperationException($"A operação atualizada com ID '{id}' não foi encontrada.");
    }

    public async Task DesativarOperacaoAsync(long id) {
        var operacao = await _sapIntegracaoRepository.ObterOperacaoPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Operação com ID '{id}' não encontrada.");

        if (!operacao.Ativo)
            return;

        await _sapIntegracaoRepository.DesativarOperacaoAsync(id);
    }
    #endregion

    /**********************************************/
    /* Métodos Privados                           */
    /**********************************************/
    #region Métodos Privados
    private async Task ProcessarIntegracaoAsync(SapIntegracaoOperacao operacao) {
        var registrosPendentes = await _sapIntegracaoRepository.ObterDadosPendentesAsync(operacao);
        if (!registrosPendentes.Any())
            return;

        var definicao = DesserializarDefinicao(operacao.JsonDefinicao);
        ValidaDefinicao(definicao);

        foreach (var registro in registrosPendentes) {
            try {
                ValidaVersao(registro, operacao);
                var pendencia = MapeiaPendencia(registro);

                await _sapIntegracaoRepository.MarcarComoProcessandoAsync(registro.Id);
                await ProcessaPendencia(pendencia, operacao, definicao);
            } catch (Exception ex) {
                var mensagemErro = ObtemMensagemErro(ex);
                await _sapIntegracaoRepository.RegistrarFalhaAsync(registro.Id, mensagemErro);
            }
        }
    }

    private async Task ProcessaPendencia(SapIntegracaoPendenteProcessamento pendencia, SapIntegracaoOperacao operacao, DefinicaoNoSap definicao) {
        var payload = await ProcessaNo(definicao, pendencia.Input, parent: null, caminhoNo: "$root")
            ?? throw new InvalidOperationException("O nó raiz não gerou conteúdo.");
        var stringJson = JsonSerializer.Serialize(payload, JsonOptions);
        // await EnviaParaSap(operacao.MetodoHttp, operacao.Endpoint, payload); //! aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
        await _sapIntegracaoRepository.MarcarComoEnviadoAsync(pendencia.Id);
    }

    private async Task<JsonNode?> ProcessaNo(DefinicaoNoSap definicao, IReadOnlyDictionary<string, object?> input, IReadOnlyDictionary<string, object?>? parent, string caminhoNo) {
        try {
            var parametros = ResolveParametros(definicao.Parametros, input, parent);
            var registros = await _sapIntegracaoRepository.ExecutarConsultaAsync(definicao.Sql, parametros);

            return definicao.Tipo switch {
                TipoNo.Object => await ProcessaObjeto(definicao, registros, input, caminhoNo),
                TipoNo.Collection => await ProcessaColecao(definicao, registros, input, caminhoNo),
                _ => throw new NotSupportedException($"Tipo de nó não suportado: {definicao.Tipo}.")
            };
        } catch (ErroProcessamentoNoException) {
            throw; //O erro já contém o caminho exato do nó que falhou.
        } catch (Exception ex) {
            throw new ErroProcessamentoNoException(caminhoNo, definicao.Tipo, ex);
        }
    }

    private async Task<JsonNode?> ProcessaObjeto(DefinicaoNoSap definicao, IEnumerable<DynamicRow> registros, IReadOnlyDictionary<string, object?> input, string caminhoNo) {
        var registrosList = registros.ToList();

        if (registrosList.Count == 0) {
            if (EhObrigatorio(definicao, caminhoNo))
                throw new InvalidOperationException("A consulta de um nó Object obrigatório não retornou registros.");

            return null;
        }

        if (registrosList.Count > 1) {
            throw new InvalidOperationException("A consulta de um nó Object retornou mais de um registro.");
        }

        return await MontaObjeto(definicao, registrosList[0], input, caminhoNo);
    }

    private async Task<JsonNode> ProcessaColecao(DefinicaoNoSap definicao, IEnumerable<DynamicRow> registros, IReadOnlyDictionary<string, object?> input, string caminhoNo) {
        var registrosList = registros.ToList();

        if (registrosList.Count == 0 && EhObrigatorio(definicao, caminhoNo))
            throw new InvalidOperationException("A consulta de um nó Collection obrigatório não retornou registros.");

        var array = new JsonArray();
        var indice = 0;

        foreach (var registro in registrosList) {
            var objeto = await MontaObjeto(definicao, registro, input, $"{caminhoNo}[{indice}]");
            array.Add(objeto);
            indice++;
        }

        return array;
    }

    private async Task<JsonObject> MontaObjeto(DefinicaoNoSap definicao, DynamicRow registro, IReadOnlyDictionary<string, object?> input, string caminhoNo) {
        ValidaCamposObrigatorios(definicao, registro, caminhoNo);

        var json = new JsonObject();
        foreach (var coluna in registro) {
            if (definicao.Excluir.Contains(coluna.Key))
                continue;
            if (coluna.Value is null || coluna.Value is DBNull)
                continue;
            json[coluna.Key] = ConverteParaJson(coluna.Value);
        }

        foreach (var filho in definicao.Filhos) {
            var nomePropriedade = filho.Key;
            var definicaoFilho = filho.Value;

            if (json.ContainsKey(nomePropriedade))
                throw new InvalidOperationException($"A propriedade '{nomePropriedade}' foi retornada pela SQL e também foi definida como um nó filho.");

            //O registro completo é passado ao filho. As colunas em Excluir continuam disponíveis para $parent.
            var caminhoFilho = $"{caminhoNo}.{nomePropriedade}";
            var valorFilho = await ProcessaNo(definicaoFilho, input, registro, caminhoFilho);

            if (definicaoFilho.OmitirSeVazio && EstaVazio(valorFilho))
                continue;

            json[nomePropriedade] = valorFilho;
        }

        return json;
    }

    private static bool EhObrigatorio(DefinicaoNoSap definicao, string caminhoNo) {
        if (caminhoNo == "$root")
            return true;

        return definicao.Obrigatorio ?? definicao.Tipo == TipoNo.Object;
    }

    private static bool EstaVazio(JsonNode? valor) {
        return valor is null
            || valor is JsonArray array && array.Count == 0
            || valor is JsonObject objeto && objeto.Count == 0;
    }

    private static void ValidaCamposObrigatorios(DefinicaoNoSap definicao, DynamicRow registro, string caminhoNo) {
        foreach (var campo in definicao.CamposObrigatorios) {
            if (!registro.TryGetValue(campo, out var valor))
                throw new InvalidOperationException($"O campo obrigatório '{caminhoNo}.{campo}' não foi retornado pela consulta SQL.");

            if (valor is null || valor is DBNull)
                throw new InvalidOperationException($"O campo obrigatório '{caminhoNo}.{campo}' retornou null.");
        }
    }

    private static Dictionary<string, object?> ResolveParametros(IReadOnlyDictionary<string, string> bindings, IReadOnlyDictionary<string, object?> input, IReadOnlyDictionary<string, object?>? parent) {
        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        foreach (var binding in bindings)
            result[binding.Key] = ResolveExpressao(binding.Value, input, parent);

        return result;
    }

    private static object? ResolveExpressao(string expression, IReadOnlyDictionary<string, object?> input, IReadOnlyDictionary<string, object?>? parent) {
        const string inputPrefix = "$input.";
        const string parentPrefix = "$parent.";

        if (expression.StartsWith(inputPrefix, StringComparison.OrdinalIgnoreCase)) {
            var name = expression[inputPrefix.Length..];

            return input.TryGetValue(name, out var value)
                ? value
                : throw new InvalidOperationException($"Parâmetro de entrada '{name}' não encontrado.");
        }

        if (expression.StartsWith(parentPrefix, StringComparison.OrdinalIgnoreCase)) {
            var name = expression[parentPrefix.Length..];

            if (parent is null)
                throw new InvalidOperationException($"Não existe nó pai para resolver '{expression}'.");

            return parent.TryGetValue(name, out var value)
                ? value
                : throw new InvalidOperationException($"Coluna '{name}' não encontrada no registro pai.");
        }

        throw new NotSupportedException($"Expressão não suportada: '{expression}'.");
    }

    private async Task EnviaParaSap(string metodoHttp, string endpoint, JsonNode payload) {

        using var request = new HttpRequestMessage(new HttpMethod(metodoHttp), endpoint);
        request.Content = JsonContent.Create(payload);
        using var response = await _httpClient.SendAsync(request); //httpcliente vai ser preparado no login

        if (response.IsSuccessStatusCode)
            return;

        var responseContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"SAP retornou HTTP {(int)response.StatusCode}: " + responseContent);
    }

    private static DefinicaoNoSap DesserializarDefinicao(string definitionJson) {
        try {
            return JsonSerializer.Deserialize<DefinicaoNoSap>(definitionJson, JsonOptions)
                ?? throw new InvalidOperationException("A definição da operação está vazia.");
        } catch (JsonException ex) {
            throw new InvalidOperationException("O JSONDEFINICAO da operação é inválido.", ex);
        }
    }

    private static SapIntegracaoPendenteProcessamento MapeiaPendencia(SapIntegracaoPendentes registro) {
        Dictionary<string, JsonElement> parametrosBrutos;

        try {
            parametrosBrutos = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(registro.InputJson, JsonOptions) ?? [];
        } catch (JsonException ex) {
            throw new InvalidOperationException($"INPUTJSON inválido na pendência '{registro.Id}'.", ex);
        }

        return new SapIntegracaoPendenteProcessamento {
            Id = registro.Id,
            IdOperacao = registro.IdOperacao,
            Input = ConverteInputJson(parametrosBrutos)
        };
    }

    private static object? ConverteJsonElement(JsonElement element) {
        return element.ValueKind switch {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => ConverteNumero(element),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => throw new NotSupportedException($"O tipo JSON '{element.ValueKind}' não pode ser usado diretamente como parâmetro SQL.")
        };
    }

    private static object ConverteNumero(JsonElement element) {
        if (element.TryGetInt32(out var intValue))
            return intValue;
        if (element.TryGetInt64(out var longValue))
            return longValue;
        if (element.TryGetDecimal(out var decimalValue))
            return decimalValue;
        return element.GetDouble();
    }

    private static JsonNode? ConverteParaJson(object? value) {
        if (value is null || value is DBNull)
            return null;

        return value switch {
            string stringValue => JsonValue.Create(stringValue),
            bool boolValue => JsonValue.Create(boolValue),
            byte byteValue => JsonValue.Create(byteValue),
            short shortValue => JsonValue.Create(shortValue),
            int intValue => JsonValue.Create(intValue),
            long longValue => JsonValue.Create(longValue),
            float floatValue => JsonValue.Create(floatValue),
            double doubleValue => JsonValue.Create(doubleValue),
            decimal decimalValue => JsonValue.Create(decimalValue),
            DateOnly dateValue => JsonValue.Create(dateValue.ToString("yyyy-MM-dd")),
            DateTime dateTimeValue => JsonValue.Create(dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ss")),
            TimeOnly timeValue => JsonValue.Create(timeValue.ToString("HH:mm:ss")),
            Guid guidValue => JsonValue.Create(guidValue.ToString()),
            _ => JsonSerializer.SerializeToNode(value, value.GetType(), JsonOptions)
        };
    }

    private static void ValidaDefinicao(DefinicaoNoSap definicao) {
        ValidaNo(definicao, caminho: "$root", profundidade: 0);
    }

    private static void ValidaNo(DefinicaoNoSap definicao, string caminho, int profundidade) {
        const int maxDepth = 10;

        if (profundidade > maxDepth)
            throw new InvalidOperationException($"A definição excedeu a profundidade máxima de {maxDepth} níveis.");
        if (string.IsNullOrWhiteSpace(definicao.Sql))
            throw new InvalidOperationException($"O nó '{caminho}' não possui SQL.");
        if (caminho == "$root" && definicao.Obrigatorio == false)
            throw new InvalidOperationException("O nó '$root' não pode ser opcional.");
        if (definicao.OmitirSeVazio && definicao.Obrigatorio == true)
            throw new InvalidOperationException($"O nó '{caminho}' não pode combinar required true com omitIfEmpty true.");

        foreach (var campo in definicao.CamposObrigatorios) {
            if (string.IsNullOrWhiteSpace(campo))
                throw new InvalidOperationException($"O nó '{caminho}' possui um campo obrigatório sem nome.");
        }

        foreach (var parametro in definicao.Parametros) {
            if (string.IsNullOrWhiteSpace(parametro.Key))
                throw new InvalidOperationException($"O nó '{caminho}' possui um parâmetro sem nome.");

            if (string.IsNullOrWhiteSpace(parametro.Value))
                throw new InvalidOperationException($"O parâmetro '{parametro.Key}' do nó '{caminho}' não possui uma expressão.");

            var naoTemInput = !parametro.Value.StartsWith("$input.", StringComparison.OrdinalIgnoreCase);
            var naoTemParent = !parametro.Value.StartsWith("$parent.", StringComparison.OrdinalIgnoreCase);
            if (naoTemInput && naoTemParent)
                throw new InvalidOperationException($"A expressão '{parametro.Value}' do nó " + $"'{caminho}' é inválida.");
        }

        foreach (var filho in definicao.Filhos) {
            if (string.IsNullOrWhiteSpace(filho.Key))
                throw new InvalidOperationException($"O nó '{caminho}' possui um filho sem nome.");

            ValidaNo(filho.Value, $"{caminho}.{filho.Key}", profundidade + 1);
        }
    }

    private static void ValidaVersao(SapIntegracaoPendentes pendencia, SapIntegracaoOperacao operacao) {
        if (pendencia.VersaoOperacao == operacao.Versao)
            return;

        var msgErro = $"A pendência foi criada para a versão {pendencia.VersaoOperacao}, mas a operação está na versão {operacao.Versao}.";
        throw new InvalidOperationException(msgErro);
    }

    private static string ObtemMensagemErro(Exception ex) {
        const int maxLength = 8_000;
        var mensagem = ex.ToString();
        return mensagem.Length <= maxLength ? mensagem : mensagem[..maxLength];
    }

    private static Dictionary<string, object?> ConverteInputJson(IReadOnlyDictionary<string, JsonElement> inputJson) {
        var input = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        foreach (var parametro in inputJson)
            input[parametro.Key] = ConverteJsonElement(parametro.Value);

        return input;
    }

    private sealed class ErroProcessamentoNoException : InvalidOperationException {
        public string CaminhoNo { get; }
        public TipoNo TipoNo { get; }

        public ErroProcessamentoNoException(string caminhoNo, TipoNo tipoNo, Exception innerException)
            : base($"Erro ao processar o nó '{caminhoNo}' ({tipoNo}): {innerException.Message}", innerException) {

            CaminhoNo = caminhoNo;
            TipoNo = tipoNo;
        }
    }
    #endregion
}
