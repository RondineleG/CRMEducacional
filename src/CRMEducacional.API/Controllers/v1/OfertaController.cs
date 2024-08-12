using CRMEducacional.Application.Dtos.Oferta.Request;
using CRMEducacional.Application.Dtos.Oferta.Response;

namespace CRMEducacional.API.Controllers.v1;

[ApiVersion("1.0")]
public class OfertaController : ApiBaseController
{
    public OfertaController(IOfertaServico ofertaServico, ILogger logger)
    {
        _ofertaServico = ofertaServico;
        _logger = logger;
    }

    private readonly ILogger _logger;

    private readonly IOfertaServico _ofertaServico;

    /// <summary>
    /// Atualiza uma oferta existente pelo ID.
    /// </summary>
    /// <param name="ofertaId">ID da oferta a ser atualizada.</param>
    /// <param name="request">Dados da oferta para atualização.</param>
    /// <returns>Retorna a oferta atualizada.</returns>
    /// <response code="200">Retorna a oferta atualizada.</response>
    /// <response code="400">Se o ID da oferta ou os dados de entrada forem inválidos.</response>
    /// <response code="404">Se a oferta não for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpPut("{ofertaId}")]
    [ProducesResponseType(typeof(OfertaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> AtualizarAsync(int ofertaId, [FromBody] OfertaRequestDto request)
    {
        if (ofertaId <= 0)
        {
            return ResponseBadRequest("ID de oferta inválido.");
        }

        if (request == null)
        {
            return ResponseBadRequest("Os dados da oferta não podem ser nulos.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(AtualizarAsync),
                $"Recebeu os parâmetros: ofertaId={ofertaId}, request={request}"
            );

            var oferta = await _ofertaServico.ObterPorIdAsync(ofertaId);
            if (oferta.Status == ECustomResultStatus.EntityNotFound)
            {
                return ResponseNotFound("Oferta não encontrada.");
            }

            var updatedOferta = OfertaRequestDto.Update(request, ofertaId);
            var response = await _ofertaServico.AtualizarAsync(updatedOferta);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(AtualizarAsync),
                $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(OfertaResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError(response.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(OfertaController),
                nameof(AtualizarAsync),
                $"Encontrou uma exceção: {ex.Message}"
            );
            return ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.");
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(AtualizarAsync),
                 $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }

    /// <summary>
    /// Cria uma nova oferta.
    /// </summary>
    /// <param name="request">Dados da oferta para criação.</param>
    /// <returns>Retorna a oferta criada.</returns>
    /// <response code="201">Retorna a oferta criada.</response>
    /// <response code="400">Se os dados de entrada forem inválidos.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(OfertaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> CriarNovoAsync([FromBody] OfertaRequestDto request)
    {
        if (request == null)
        {
            return ResponseBadRequest("A oferta não pode ser nula.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(CriarNovoAsync),
                $"Recebeu os parâmetros: request={request}"
            );

            var oferta = OfertaRequestDto.Create(request);
            var response = await _ofertaServico.AdicionarAsync(oferta);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(CriarNovoAsync),
                $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseCreated(OfertaResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(OfertaController),
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
                nameof(OfertaController),
                nameof(CriarNovoAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Exclui uma oferta existente pelo ID.
    /// </summary>
    /// <param name="ofertaId">ID da oferta a ser excluída.</param>
    /// <returns>Retorna o status da exclusão.</returns>
    /// <response code="200">Se a exclusão for bem-sucedida.</response>
    /// <response code="400">Se o ID da oferta for inválido.</response>
    /// <response code="404">Se a oferta não for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpDelete("{ofertaId}")]
    [ProducesResponseType(typeof(OfertaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletarAsync(int ofertaId)
    {
        if (ofertaId <= 0)
        {
            return ResponseBadRequest("ID de oferta inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(DeletarAsync),
                $"Recebeu o parâmetro: ofertaId={ofertaId}"
            );

            var resultado = await _ofertaServico.RemoverAsync(ofertaId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(DeletarAsync),
                $"Exclusão concluída com sucesso para o ofertaId={ofertaId}"
            );

            return resultado.Status switch
            {
                ECustomResultStatus.Success => ResponseNoContent(),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(resultado.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => ResponseBadRequest(resultado.Error.Description),
                _ => ResponseInternalServerError(resultado.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(OfertaController),
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
                nameof(OfertaController),
                nameof(DeletarAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Obtém uma oferta existente pelo ID.
    /// </summary>
    /// <param name="ofertaId">ID da oferta a ser obtida.</param>
    /// <returns>Retorna a oferta encontrada.</returns>
    /// <response code="200">Retorna a oferta encontrada.</response>
    /// <response code="400">Se o ID da oferta for inválido.</response>
    /// <response code="404">Se a oferta não for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("{ofertaId}")]
    [ProducesResponseType(typeof(OfertaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPorIdAsync(int ofertaId)
    {
        if (ofertaId <= 0)
        {
            return ResponseBadRequest("ID de oferta inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(ObterPorIdAsync),
                $"Recebeu o parâmetro: ofertaId={ofertaId}"
            );

            var response = await _ofertaServico.ObterPorIdAsync(ofertaId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(ObterPorIdAsync),
                            $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"

            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(OfertaResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(OfertaController),
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
                nameof(OfertaController),
                nameof(ObterPorIdAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Retorna todas as ofertas cadastradas.
    /// </summary>
    /// <param name="pagina">Número da página (padrão: 1).</param>
    /// <param name="itensPorPagina">Número de itens por página (padrão: 20).</param>
    /// <returns>Retorna as ofertas encontradas.</returns>
    /// <response code="200">Retorna as ofertas encontradas.</response>
    /// <response code="400">Se os parâmetros de paginação forem inválidos.</response>
    /// <response code="404">Se nenhuma oferta for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("Todos")]
    [ProducesResponseType(typeof(IEnumerable<OfertaResponseDto>), StatusCodes.Status200OK)]
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
                nameof(OfertaController),
                nameof(ObterTodosAsync),
                $"Recebeu os parâmetros: pagina={pagina}, itensPorPagina={itensPorPagina}"
            );

            var response = await _ofertaServico.ObterTodosAsync(pagina, itensPorPagina);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(OfertaController),
                nameof(ObterTodosAsync),
               $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(response.Data?.Select(OfertaResponseDto.FromEntity)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(OfertaController),
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
                nameof(OfertaController),
                nameof(ObterTodosAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }
}