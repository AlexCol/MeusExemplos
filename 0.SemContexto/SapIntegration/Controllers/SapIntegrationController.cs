using Microsoft.AspNetCore.Mvc;

namespace CSConnect.SapIntegration;

[ApiController]
[Route("api/[controller]")]
public class SapIntegrationController : ControllerBase {
    private readonly ISapIntegracaoService _sapIntegrationService;

    public SapIntegrationController(ISapIntegracaoService sapIntegrationService) {
        _sapIntegrationService = sapIntegrationService;
    }

    [HttpPost("processar")]
    public async Task<IActionResult> Processar() {
        try {
            await _sapIntegrationService.ProcessarIntegracoesAsync();
            return Ok("Integração com o SAP processada com sucesso.");
        } catch (Exception ex) {
            return StatusCode(500, $"Erro ao processar a integração com o SAP: {ex.Message}");
        }
    }

    [HttpPost("operacoes/{idOperacao:long}/gerar-payload")]
    public async Task<IActionResult> GerarPayload(long idOperacao, [FromBody] Dictionary<string, JsonElement> inputJson) {
        try {
            var payload = await _sapIntegrationService.GerarPayloadAsync(idOperacao, inputJson);
            return Ok(payload);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        }
    }

    /**********************************************/
    /* CRUD das Operações                         */
    /**********************************************/
    #region CRUD das Operações
    [HttpGet("operacoes")]
    public async Task<IActionResult> ObterOperacoes() {
        var operacoes = await _sapIntegrationService.ObterOperacoesAsync();
        return Ok(operacoes);
    }

    [HttpGet("operacoes/{id:long}")]
    public async Task<IActionResult> ObterOperacaoPorId(long id) {
        var operacao = await _sapIntegrationService.ObterOperacaoPorIdAsync(id);
        return operacao is null ? NotFound() : Ok(operacao);
    }

    [HttpPost("operacoes")]
    public async Task<IActionResult> CriarOperacao([FromBody] CriarSapIntegracaoOperacaoDto dto) {
        try {
            var operacao = await _sapIntegrationService.CriarOperacaoAsync(dto);
            return CreatedAtAction(nameof(ObterOperacaoPorId), new { id = operacao.Id }, operacao);
        } catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("operacoes/{id:long}")]
    public async Task<IActionResult> AtualizarOperacao(long id, [FromBody] AtualizarSapIntegracaoOperacaoDto dto) {
        try {
            var operacao = await _sapIntegrationService.AtualizarOperacaoAsync(id, dto);
            return Ok(operacao);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (InvalidOperationException ex) {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("operacoes/{id:long}")]
    public async Task<IActionResult> DesativarOperacao(long id) {
        try {
            await _sapIntegrationService.DesativarOperacaoAsync(id);
            return NoContent();
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        }
    }
    #endregion

}
