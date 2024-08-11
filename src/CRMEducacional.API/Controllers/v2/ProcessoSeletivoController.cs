using CRMEducacional.Application.Dtos.ProcessoSeletivo.Response;

namespace CRMEducacional.API.Controllers.v2;

[ApiVersion("2.0")]
public class ProcessoSeletivoController : ApiBaseController
{
    public ProcessoSeletivoController(
        IProcessoSeletivoServico processoSeletivoServico,
        ILogger logger)
    {
        _processoSeletivoServico = processoSeletivoServico;
        _logger = logger;
    }

    private readonly ILogger _logger;

    private readonly IProcessoSeletivoServico _processoSeletivoServico;

    /// <summary>
    /// Retorna todos os processos seletivos cadastrados.
    /// </summary>
    /// <param name="pagina">Número da página (padrão: 1).</param>
    /// <param name="itensPorPagina">Número de itens por página (padrão: 20).</param>
    /// <returns>Retorna os processos seletivos encontrados.</returns>
    /// <response code="200">Retorna os processos seletivos encontrados.</response>
    /// <response code="400">Se os parâmetros de paginação forem inválidos.</response>
    /// <response code="404">Se nenhum processo seletivo for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("Todos")]
    [ProducesResponseType(typeof(IEnumerable<ProcessoSeletivoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public async Task<IActionResult> ObterTodosAsync([FromQuery] int pagina = 1, [FromQuery] int itensPorPagina = 20)
    {
        if (pagina <= 0 || itensPorPagina <= 0)
        {
            return ResponseBadRequest("Os parâmetros de paginação devem ser maiores que zero.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(ObterTodosAsync),
                "Recebeu os parâmetros: pagina={Pagina}, itensPorPagina={ItensPorPagina}",
                pagina,
                itensPorPagina
            );

            var response = await _processoSeletivoServico.ObterTodosAsync(pagina, itensPorPagina);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(ObterTodosAsync),
                "Completou a execução. Status: {Status}. Tempo de execução: {TempoExecucao} ms",
                response.Status,
                stopwatch.ElapsedMilliseconds
            );

            var dtoResponse = response.Data?.Select(ProcessoSeletivoResponseDto.FromEntity);

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(dtoResponse),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(ProcessoSeletivoController),
                nameof(ObterTodosAsync),
                "Encontrou uma exceção: {MensagemExcecao}",
                ex.Message
            );
            return ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.");
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(ObterTodosAsync),
                "Tempo total de execução: {TempoExecucao} ms",
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}