using CRMEducacional.Application.Dtos.ProcessoSeletivo.Request;
using CRMEducacional.Application.Dtos.ProcessoSeletivo.Response;

namespace CRMEducacional.API.Controllers.v1;

[ApiVersion("1.0")]
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
    /// Atualiza um processo seletivo existente pelo ID.
    /// </summary>
    /// <param name="processoSeletivoId">ID do processo seletivo a ser atualizado.</param>
    /// <param name="request">Dados do processo seletivo para atualização.</param>
    /// <returns>Retorna o processo seletivo atualizado.</returns>
    /// <response code="200">Retorna o processo seletivo atualizado.</response>
    /// <response code="400">Se o ID do processo seletivo ou os dados de entrada forem inválidos.</response>
    /// <response code="404">Se o processo seletivo não for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpPut("{processoSeletivoId}")]
    [ProducesResponseType(typeof(ProcessoSeletivoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> AtualizarAsync(int processoSeletivoId, [FromBody] ProcessoSeletivoRequestDto request)
    {
        if (processoSeletivoId <= 0)
        {
            return ResponseBadRequest("ID de processo seletivo inválido.");
        }

        if (request == null)
        {
            return ResponseBadRequest("Os dados do processo seletivo não podem ser nulos.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(AtualizarAsync),
                $"Recebeu os parâmetros: processoSeletivoId={processoSeletivoId}, request={request}"
            );

            var processoSeletivo = await _processoSeletivoServico.ObterPorIdAsync(processoSeletivoId);
            if (processoSeletivo.Status == ECustomResultStatus.EntityNotFound)
            {
                return ResponseNotFound("Processo seletivo não encontrado.");
            }

            var updatedProcessoSeletivo = ProcessoSeletivoRequestDto.Update(request, processoSeletivoId);
            var response = await _processoSeletivoServico.AtualizarAsync(updatedProcessoSeletivo);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(AtualizarAsync),
                              $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"

            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(ProcessoSeletivoResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError(response.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(ProcessoSeletivoController),
                nameof(AtualizarAsync),
                $"Encontrou uma exceção: {ex.Message}"
            );
            return ResponseInternalServerError(
                "Ocorreu um erro interno ao processar a solicitação."
            );
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(AtualizarAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Cria um novo processo seletivo.
    /// </summary>
    /// <param name="request">Dados do processo seletivo para criação.</param>
    /// <returns>Retorna o processo seletivo criado.</returns>
    /// <response code="201">Retorna o processo seletivo criado.</response>
    /// <response code="400">Se os dados de entrada forem inválidos.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProcessoSeletivoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> CriarNovoAsync([FromBody] ProcessoSeletivoRequestDto request)
    {
        if (request == null)
        {
            return ResponseBadRequest("O processo seletivo não pode ser nulo.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(CriarNovoAsync),
                $"Recebeu os parâmetros: request={request}"
            );

            var processoSeletivo = ProcessoSeletivoRequestDto.Create(request);
            var response = await _processoSeletivoServico.AdicionarAsync(processoSeletivo);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(CriarNovoAsync),
                               $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"

            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseCreated(ProcessoSeletivoResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(ProcessoSeletivoController),
                nameof(CriarNovoAsync),
                $"Encontrou uma exceção: {ex.Message}"
            );
            return ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.");
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(CriarNovoAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Exclui um processo seletivo existente pelo ID.
    /// </summary>
    /// <param name="processoSeletivoId">ID do processo seletivo a ser excluído.</param>
    /// <returns>Retorna o status da exclusão.</returns>
    /// <response code="200">Se a exclusão for bem-sucedida.</response>
    /// <response code="400">Se o ID do processo seletivo for inválido.</response>
    /// <response code="404">Se o processo seletivo não for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpDelete("{processoSeletivoId}")]
    [ProducesResponseType(typeof(ProcessoSeletivoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletarAsync(int processoSeletivoId)
    {
        if (processoSeletivoId <= 0)
        {
            return ResponseBadRequest("ID de processo seletivo inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(DeletarAsync),
                $"Recebeu o parâmetro: processoSeletivoId={processoSeletivoId}"
            );

            var response = await _processoSeletivoServico.RemoverAsync(processoSeletivoId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(DeletarAsync),
                $"Exclusão concluída com sucesso para o processoSeletivoId={processoSeletivoId}"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseNoContent(),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => ResponseBadRequest(response.Error.Description),
                _ => ResponseInternalServerError(response.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(ProcessoSeletivoController),
                nameof(DeletarAsync),
                $"Encontrou uma exceção: {ex.Message}"
            );
            return ResponseInternalServerError(
                "Ocorreu um erro interno ao processar a solicitação."
            );
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(DeletarAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Obtém um processo seletivo pelo ID.
    /// </summary>
    /// <param name="processoSeletivoId">ID do processo seletivo a ser obtido.</param>
    /// <returns>Retorna o processo seletivo encontrado.</returns>
    /// <response code="200">Retorna o processo seletivo encontrado.</response>
    /// <response code="400">Se o ID do processo seletivo for inválido.</response>
    /// <response code="404">Se o processo seletivo não for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("{processoSeletivoId}")]
    [ProducesResponseType(typeof(ProcessoSeletivoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPorIdAsync(int processoSeletivoId)
    {
        if (processoSeletivoId <= 0)
        {
            return ResponseBadRequest("ID de processo seletivo inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(ObterPorIdAsync),
                $"Recebeu o parâmetro: processoSeletivoId={processoSeletivoId}"
            );

            var response = await _processoSeletivoServico.ObterPorIdAsync(processoSeletivoId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(ObterPorIdAsync),
                              $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"

            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(ProcessoSeletivoResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError(response.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(ProcessoSeletivoController),
                nameof(ObterPorIdAsync),
                $"Encontrou uma exceção: {ex.Message}"
            );
            return ResponseInternalServerError(
                "Ocorreu um erro interno ao processar a solicitação."
            );
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(ObterPorIdAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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
                $"Recebeu os parâmetros: pagina={pagina}, itensPorPagina={itensPorPagina}"
            );

            var response = await _processoSeletivoServico.ObterTodosAsync(pagina, itensPorPagina);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(ProcessoSeletivoController),
                nameof(ObterTodosAsync),
                               $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"

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
                $"Encontrou uma exceção: {ex.Message}"
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
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }
}