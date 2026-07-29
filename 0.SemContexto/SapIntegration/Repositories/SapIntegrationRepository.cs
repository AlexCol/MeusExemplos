namespace CSConnect.SapIntegration;

public interface ISapIntegracaoRepository {
    /*metodos para CRUD de Operacoes */
    Task<IEnumerable<SapIntegracaoOperacao>> ObterTodasOperacoesAsync();
    Task<SapIntegracaoOperacao?> ObterOperacaoPorIdAsync(long id);
    Task<long> CriarOperacaoAsync(CriarSapIntegracaoOperacaoDto dto, string jsonDefinicao);
    Task AtualizarOperacaoAsync(long id, AtualizarSapIntegracaoOperacaoDto dto, string jsonDefinicao);
    Task DesativarOperacaoAsync(long id);
    /*metodos para processamento dos endpoints */

    Task<IEnumerable<SapIntegracaoOperacao>> ObterOperacoesAtivasAsync();
    Task<IEnumerable<SapIntegracaoPendentes>> ObterDadosPendentesAsync(SapIntegracaoOperacao operacao);
    Task<IEnumerable<DynamicRow>> ExecutarConsultaAsync(string sql, IReadOnlyDictionary<string, object?> parametros);
    Task MarcarComoProcessandoAsync(long idPendencia);
    Task MarcarComoEnviadoAsync(long idPendencia);
    Task RegistrarFalhaAsync(long idPendencia, string mensagemErro);
}

public class SapIntegracaoRepository : ISapIntegracaoRepository {
    private readonly IFirebirdConnectionFactory _connection;
    public SapIntegracaoRepository(IFirebirdConnectionFactory connection) {
        _connection = connection;
    }

    /**********************************************/
    /* CRUD das Operações                         */
    /**********************************************/
    #region CRUD das Operações
    public async Task<IEnumerable<SapIntegracaoOperacao>> ObterTodasOperacoesAsync() {
        const string sql = @"
            SELECT
                ID,
                NOME,
                ENDPOINT,
                METODOHTTP,
                JSONDEFINICAO,
                VERSAO,
                ATIVO
            FROM TB_SAP_INTEGRACAO_OPERACAO
            ORDER BY NOME";

        return await _connection.QueryAsync<SapIntegracaoOperacao>(sql);
    }

    public async Task<SapIntegracaoOperacao?> ObterOperacaoPorIdAsync(long id) {
        const string sql = @"
            SELECT
                ID,
                NOME,
                ENDPOINT,
                METODOHTTP,
                JSONDEFINICAO,
                VERSAO,
                ATIVO
            FROM TB_SAP_INTEGRACAO_OPERACAO
            WHERE ID = @Id";

        var operacoes = await _connection.QueryAsync<SapIntegracaoOperacao>(sql, new { Id = id });
        return operacoes.FirstOrDefault();
    }

    public async Task<long> CriarOperacaoAsync(CriarSapIntegracaoOperacaoDto dto, string jsonDefinicao) {
        const string sql = @"
            INSERT INTO TB_SAP_INTEGRACAO_OPERACAO (
                NOME, ENDPOINT, METODOHTTP, JSONDEFINICAO, VERSAO, ATIVO
            ) VALUES (
                @Nome, @Endpoint, @MetodoHttp, @JsonDefinicao, 1, @Ativo
            )
            RETURNING ID";

        var ids = await _connection.QueryAsync<long>(sql, new {
            dto.Nome,
            dto.Endpoint,
            MetodoHttp = dto.MetodoHttp.ToUpperInvariant(),
            JsonDefinicao = jsonDefinicao,
            dto.Ativo
        });

        return ids.Single();
    }

    public async Task AtualizarOperacaoAsync(long id, AtualizarSapIntegracaoOperacaoDto dto, string jsonDefinicao) {
        const string sql = @"
            UPDATE TB_SAP_INTEGRACAO_OPERACAO
            SET NOME = @Nome,
                ENDPOINT = @Endpoint,
                METODOHTTP = @MetodoHttp,
                JSONDEFINICAO = @JsonDefinicao,
                VERSAO = VERSAO + 1,
                ATIVO = @Ativo
            WHERE ID = @Id
              AND VERSAO = @Versao";

        await _connection.ExecuteAsync(sql, new {
            Id = id,
            dto.Nome,
            dto.Endpoint,
            MetodoHttp = dto.MetodoHttp.ToUpperInvariant(),
            JsonDefinicao = jsonDefinicao,
            dto.Ativo,
            dto.Versao
        });
    }

    public async Task DesativarOperacaoAsync(long id) {
        const string sql = @"
            UPDATE TB_SAP_INTEGRACAO_OPERACAO
            SET ATIVO = FALSE
            WHERE ID = @Id
              AND ATIVO = TRUE";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }
    #endregion

    /**********************************************/
    /* Metodos de Processamento dos Endpoints     */
    /**********************************************/
    #region Metodos de Processamento dos Endpoints
    public async Task<IEnumerable<SapIntegracaoOperacao>> ObterOperacoesAtivasAsync() {
        const string sql = @"
            SELECT 
                ID,
                NOME,
                ENDPOINT,
                METODOHTTP,
                JSONDEFINICAO,
                VERSAO,
                ATIVO
            FROM TB_SAP_INTEGRACAO_OPERACAO
            WHERE ATIVO = TRUE
            ORDER BY ID";

        var result = await _connection.QueryAsync<SapIntegracaoOperacao>(sql);
        return result;
    }

    public async Task<IEnumerable<SapIntegracaoPendentes>> ObterDadosPendentesAsync(SapIntegracaoOperacao operacao) {
        const string sql = @"
            SELECT
                ID,
                IDOPERACAO,
                INPUTJSON,
                STATUS,
                TENTATIVAS,
                DHCRIACAO,
                DHULTIMATENTATIVA,
                DHENVIADO,
                MENSAGEMERRO,
                VERSAOOPERACAO
            FROM TB_SAP_INTEGRACAO_PENDENTES
            WHERE IDOPERACAO = @IdOperacao
              AND STATUS IN ('PENDING', 'FAILED')
            ORDER BY DHCRIACAO";

        var result = await _connection.QueryAsync<SapIntegracaoPendentes>(sql, new { IdOperacao = operacao.Id });
        return result;
    }

    public async Task<IEnumerable<DynamicRow>> ExecutarConsultaAsync(string sql, IReadOnlyDictionary<string, object?> parametros) {
        var rows = await _connection.QueryAsync<dynamic>(sql, parametros);
        return rows.Select(MapearLinhaDinamica);
    }

    public async Task MarcarComoProcessandoAsync(long idPendencia) {
        const string sql = @"
            UPDATE TB_SAP_INTEGRACAO_PENDENTES
            SET STATUS = 'PROCESSING',
                TENTATIVAS = TENTATIVAS + 1,
                DHULTIMATENTATIVA = CURRENT_TIMESTAMP,
                MENSAGEMERRO = NULL
            WHERE ID = @Id";

        await _connection.ExecuteAsync(sql, new { Id = idPendencia });
    }

    public async Task MarcarComoEnviadoAsync(long idPendencia) {
        const string sql = @"
            UPDATE TB_SAP_INTEGRACAO_PENDENTES
                SET STATUS = 'SENT',
                    DHENVIADO = CURRENT_TIMESTAMP,
                    MENSAGEMERRO = NULL
             WHERE ID = @Id";

        await _connection.ExecuteAsync(sql, new { Id = idPendencia });
    }

    public async Task RegistrarFalhaAsync(long idPendencia, string mensagemErro) {
        const string sql = @"
            UPDATE TB_SAP_INTEGRACAO_PENDENTES
                SET STATUS = 'FAILED',
                    DHULTIMATENTATIVA = CURRENT_TIMESTAMP,
                    MENSAGEMERRO = @MensagemErro
             WHERE ID = @Id
            ";

        await _connection.ExecuteAsync(sql, new { Id = idPendencia, MensagemErro = mensagemErro });
    }

    private static DynamicRow MapearLinhaDinamica(dynamic row) {
        if (row is not IDictionary<string, object> values) {
            throw new InvalidOperationException("O resultado da consulta dinâmica não implementa IDictionary<string, object>.");
        }

        var result = new DynamicRow();
        foreach (var value in values) {
            result[value.Key] = value.Value;
        }
        return result;
    }
    #endregion
}
