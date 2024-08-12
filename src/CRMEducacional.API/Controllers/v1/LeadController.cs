using CRMEducacional.Application.Dtos.Lead.Request;
using CRMEducacional.Application.Dtos.Lead.Response;

namespace CRMEducacional.API.Controllers.v1;

[ApiVersion("1.0")]
public class LeadController : ApiBaseController
{
    public LeadController(ILeadServico leadServico, ILogger logger)
    {
        _leadServico = leadServico;
        _logger = logger;
    }

    private readonly ILeadServico _leadServico;

    private readonly ILogger _logger;

    /// <summary>
    /// Atualiza um lead existente pelo ID.
    /// </summary>
    /// <param name="leadId">ID do lead a ser atualizado.</param>
    /// <param name="request">Dados do lead para atualização.</param>
    /// <returns>Retorna o lead atualizado.</returns>
    /// <response code="200">Retorna o lead atualizado.</response>
    /// <response code="400">Se o ID do lead ou os dados de entrada forem inválidos.</response>
    /// <response code="404">Se o lead não for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpPut("{leadId}")]
    [ProducesResponseType(typeof(LeadResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> AtualizarAsync(int leadId, [FromBody] LeadRequestDto request)
    {
        if (leadId <= 0)
        {
            return ResponseBadRequest("ID de lead inválido.");
        }

        if (request == null)
        {
            return ResponseBadRequest("Os dados do lead não podem ser nulos.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(AtualizarAsync),
                $"Recebeu os parâmetros: leadId={leadId}, request={request}"
            );

            var lead = await _leadServico.ObterPorIdAsync(leadId);
            if (lead.Status == ECustomResultStatus.EntityNotFound)
            {
                return ResponseNotFound("Lead não encontrado.");
            }

            var updatedLead = LeadRequestDto.Update(request, leadId);
            var response = await _leadServico.AtualizarAsync(updatedLead);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(AtualizarAsync),
               $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(LeadResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError(response.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(LeadController),
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
                nameof(LeadController),
                nameof(AtualizarAsync),
                $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }

    /// <summary>
    /// Cria um novo lead.
    /// </summary>
    /// <param name="request">Dados do lead para criação.</param>
    /// <returns>Retorna o lead criado.</returns>
    /// <response code="201">Retorna o lead criado.</response>
    /// <response code="400">Se os dados de entrada forem inválidos.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(LeadResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> CriarNovoAsync([FromBody] LeadRequestDto request)
    {
        if (request == null)
        {
            return ResponseBadRequest("O lead não pode ser nulo.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(CriarNovoAsync),
                $"Recebeu os parâmetros: request={request}"
            );

            var lead = LeadRequestDto.Create(request);
            var response = await _leadServico.AdicionarAsync(lead);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(CriarNovoAsync),
               $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseCreated(LeadResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(LeadController),
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
                nameof(LeadController),
                nameof(CriarNovoAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Exclui um lead existente pelo ID.
    /// </summary>
    /// <param name="leadId">ID do lead a ser excluído.</param>
    /// <returns>Retorna o status da exclusão.</returns>
    /// <response code="200">Se a exclusão for bem-sucedida.</response>
    /// <response code="400">Se o ID do lead for inválido.</response>
    /// <response code="404">Se o lead não for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpDelete("{leadId}")]
    [ProducesResponseType(typeof(LeadResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletarAsync(int leadId)
    {
        if (leadId <= 0)
        {
            return ResponseBadRequest("ID de lead inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(DeletarAsync),
                $"Recebeu o parâmetro: leadId={leadId}"
            );

            var resultado = await _leadServico.RemoverAsync(leadId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(DeletarAsync),
                $"Exclusão concluída com sucesso para o leadId={leadId}"
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
                nameof(LeadController),
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
                nameof(LeadController),
                nameof(DeletarAsync),
               $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }

    /// <summary>
    /// Retorna um lead pelo ID.
    /// </summary>
    /// <param name="cpf">CPF do lead a ser retornado.</param>
    /// <returns>Retorna o lead correspondente ao ID.</returns>
    /// <response code="200">Retorna o lead encontrado.</response>
    /// <response code="400">Se o ID do lead for inválido.</response>
    /// <response code="404">Se o lead não for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("cpf/{cpf}")]
    [ProducesResponseType(typeof(LeadResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPorCPFAsync(string cpf)
    {
        if (string.IsNullOrEmpty(cpf))
        {
            return ResponseBadRequest("O CPF de lead não pdoe ser nulo.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(ObterPorIdAsync),
               $"Recebeu o parâmetro: cpf={cpf}"
            );

            var response = await _leadServico.ObterPorCPFAsync(cpf);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(ObterPorIdAsync),
                $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(LeadResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(LeadController),
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
                nameof(LeadController),
                nameof(ObterPorIdAsync),
                $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }

    /// <summary>
    /// Retorna um lead pelo ID.
    /// </summary>
    /// <param name="leadId">ID do lead a ser retornado.</param>
    /// <returns>Retorna o lead correspondente ao ID.</returns>
    /// <response code="200">Retorna o lead encontrado.</response>
    /// <response code="400">Se o ID do lead for inválido.</response>
    /// <response code="404">Se o lead não for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("leadId/{leadId:int}")]
    [ProducesResponseType(typeof(LeadResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPorIdAsync(int leadId)
    {
        if (leadId <= 0)
        {
            return ResponseBadRequest("ID de lead inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(ObterPorIdAsync),
                $"Recebeu o parâmetro: leadId={leadId}"
            );

            var response = await _leadServico.ObterPorIdAsync(leadId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(ObterPorIdAsync),
                $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(LeadResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(LeadController),
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
                nameof(LeadController),
                nameof(ObterPorIdAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Retorna todos os leads cadastrados.
    /// </summary>
    /// <param name="pagina">Número da página (padrão: 1).</param>
    /// <param name="itensPorPagina">Número de itens por página (padrão: 20).</param>
    /// <returns>Retorna os leads encontrados.</returns>
    /// <response code="200">Retorna os leads encontrados.</response>
    /// <response code="400">Se os parâmetros de paginação forem inválidos.</response>
    /// <response code="404">Se nenhum lead for encontrado.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LeadResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
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
                nameof(LeadController),
                nameof(ObterTodosAsync),
                $"Recebeu os parâmetros: pagina={pagina}, itensPorPagina={itensPorPagina}"
            );

            var response = await _leadServico.ObterTodosAsync(pagina, itensPorPagina);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(LeadController),
                nameof(ObterTodosAsync),
                $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(response.Data?.Select(LeadResponseDto.FromEntity)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(LeadController),
                nameof(ObterTodosAsync),
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
                nameof(LeadController),
                nameof(ObterTodosAsync),
         $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }
}