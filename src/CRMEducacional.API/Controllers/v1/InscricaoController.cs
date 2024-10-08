﻿using CRMEducacional.Application.Dtos.Inscricao.Request;
using CRMEducacional.Application.Dtos.Inscricao.Response;

namespace CRMEducacional.API.Controllers.v1;

[ApiVersion("1.0")]
public class InscricaoController : ApiBaseController
{
    public InscricaoController(IInscricaoServico inscricaoServico, ILogger logger)
    {
        _inscricaoServico = inscricaoServico;
        _logger = logger;
    }

    private readonly IInscricaoServico _inscricaoServico;

    private readonly ILogger _logger;


    /// <summary>
    /// Obtém todas as inscrições associadas a um determinado CPF.
    /// </summary>
    /// <param name="cpf">CPF do lead.</param>
    /// <returns>Retorna as inscrições associadas ao CPF.</returns>
    /// <response code="200">Retorna as inscrições encontradas.</response>
    /// <response code="400">Se o CPF for inválido.</response>
    /// <response code="404">Se nenhuma inscrição for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("PorCpf/{cpf}")]
    [ProducesResponseType(typeof(IEnumerable<InscricaoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPorCPFAsync(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return ResponseBadRequest("CPF não pode ser nulo ou vazio.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterPorCPFAsync),
                $"Recebeu o parâmetro: cpf={cpf}"
            );

            var response = await _inscricaoServico.ObterPorCPFAsync(cpf);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterPorCPFAsync),
                $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(response.Data?.Select(InscricaoResponseDto.FromEntity)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound("Nenhuma inscrição encontrada para o CPF informado."),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(InscricaoController),
                nameof(ObterPorCPFAsync),
                $"Encontrou uma exceção: {ex.Message}"
            );
            return ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.");
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterPorCPFAsync),
                $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }

    /// <summary>
    /// Obtém todas as inscrições associadas a uma determinada oferta.
    /// </summary>
    /// <param name="ofertaId">ID da oferta.</param>
    /// <returns>Retorna as inscrições associadas à oferta.</returns>
    /// <response code="200">Retorna as inscrições encontradas.</response>
    /// <response code="400">Se o ID da oferta for inválido.</response>
    /// <response code="404">Se nenhuma inscrição for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("PorOferta/{ofertaId}")]
    [ProducesResponseType(typeof(IEnumerable<InscricaoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPorOfertaAsync(int ofertaId)
    {
        if (ofertaId <= 0)
        {
            return ResponseBadRequest("ID da oferta inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterPorOfertaAsync),
                $"Recebeu o parâmetro: ofertaId={ofertaId}"
            );

            var response = await _inscricaoServico.ObterPorOfertaAsync(ofertaId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterPorOfertaAsync),
                $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(response.Data?.Select(InscricaoResponseDto.FromEntity)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound("Nenhuma inscrição encontrada para o ID da oferta informado."),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(InscricaoController),
                nameof(ObterPorOfertaAsync),
                $"Encontrou uma exceção: {ex.Message}"
            );
            return ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.");
        }
        finally
        {
            stopwatch.Stop();
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterPorOfertaAsync),
                $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }


    /// <summary>
    /// Cria uma nova inscrição.
    /// </summary>
    /// <param name="request">Dados da inscrição para criação.</param>
    /// <returns>Retorna a inscrição criada.</returns>
    /// <response code="201">Retorna a inscrição criada.</response>
    /// <response code="400">Se os dados de entrada forem inválidos.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(InscricaoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> CriarNovoAsync([FromBody] InscricaoRequestDto request)
    {
        if (request == null)
        {
            return ResponseBadRequest("A inscrição não pode ser nula.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(CriarNovoAsync),
                $"Recebeu os parâmetros: request={request}"
            );

            var inscricao = InscricaoRequestDto.Create(request);
            var response = await _inscricaoServico.AdicionarAsync(inscricao);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(CriarNovoAsync),
               $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseCreated(InscricaoResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(InscricaoController),
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
                nameof(InscricaoController),
                nameof(CriarNovoAsync),
                $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Atualiza uma inscrição existente pelo ID.
    /// </summary>
    /// <param name="inscricaoId">ID da inscrição a ser atualizada.</param>
    /// <param name="inscricaoRequestDto">Dados da inscrição para atualização.</param>
    /// <returns>Retorna a inscrição atualizada.</returns>
    /// <response code="200">Retorna a inscrição atualizada.</response>
    /// <response code="400">Se o ID da inscrição ou os dados de entrada forem inválidos.</response>
    /// <response code="404">Se a inscrição não for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpPut("{inscricaoId}")]
    [ProducesResponseType(typeof(InscricaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> AtualizarAsync(int inscricaoId, [FromBody] InscricaoRequestDto inscricaoRequestDto)
    {
        if (inscricaoId <= 0)
        {
            return ResponseBadRequest("ID de inscrição inválido.");
        }

        if (inscricaoRequestDto == null)
        {
            return ResponseBadRequest("Os dados da inscrição não podem ser nulos.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(AtualizarAsync),
               $"Recebeu os parâmetros: inscricaoId={inscricaoId}, inscricaoRequestDto={inscricaoRequestDto}"
            );

            var inscricaoResult = await _inscricaoServico.ObterPorIdAsync(inscricaoId);
            if (inscricaoResult.Status == ECustomResultStatus.EntityNotFound)
            {
                return ResponseNotFound("Inscrição não encontrada.");
            }

            var request = new InscricaoUpdateRequestDto
            {
                Id = inscricaoId,
                Inscricao = inscricaoRequestDto
            };

            var inscricao = InscricaoRequestDto.Update(request.Inscricao, request.Id);
            var response = await _inscricaoServico.AtualizarAsync(inscricao);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(AtualizarAsync),
             $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(InscricaoResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError(response.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(InscricaoController),
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
                nameof(InscricaoController),
                nameof(AtualizarAsync),
                $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }

    /// <summary>
    /// Exclui uma inscrição existente pelo ID.
    /// </summary>
    /// <param name="inscricaoId">ID da inscrição a ser excluída.</param>
    /// <returns>Retorna o status da exclusão.</returns>
    /// <response code="200">Se a exclusão for bem-sucedida.</response>
    /// <response code="400">Se o ID da inscrição for inválido.</response>
    /// <response code="404">Se a inscrição não for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpDelete("{inscricaoId}")]
    [ProducesResponseType(typeof(InscricaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomValidationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomValidationResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomValidationResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletarAsync(int inscricaoId)
    {
        if (inscricaoId <= 0)
        {
            return ResponseBadRequest("ID de inscrição inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(DeletarAsync),
                $"Recebeu o parâmetro: inscricaoId={inscricaoId}"
            );

            var response = await _inscricaoServico.RemoverAsync(inscricaoId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(DeletarAsync),
                $"Exclusão concluída com sucesso para o inscricaoId={inscricaoId}"
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
                nameof(InscricaoController),
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
                nameof(InscricaoController),
                nameof(DeletarAsync),
               $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"

            );
        }
    }

    /// <summary>
    /// Obtém uma inscrição existente pelo ID.
    /// </summary>
    /// <param name="inscricaoId">ID da inscrição a ser obtida.</param>
    /// <returns>Retorna a inscrição encontrada.</returns>
    /// <response code="200">Retorna a inscrição encontrada.</response>
    /// <response code="400">Se o ID da inscrição for inválido.</response>
    /// <response code="404">Se a inscrição não for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("{inscricaoId}")]
    [ProducesResponseType(typeof(InscricaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPorIdAsync(int inscricaoId)
    {
        if (inscricaoId <= 0)
        {
            return ResponseBadRequest("ID de inscrição inválido.");
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterPorIdAsync),
                $"Recebeu o parâmetro: inscricaoId={inscricaoId}"
            );

            var response = await _inscricaoServico.ObterPorIdAsync(inscricaoId);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterPorIdAsync),
                        $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(InscricaoResponseDto.FromEntity(response.Data)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(InscricaoController),
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
                nameof(InscricaoController),
                nameof(ObterPorIdAsync),
               $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }

    /// <summary>
    /// Retorna todas as inscrições cadastradas.
    /// </summary>
    /// <param name="pagina">Número da página (padrão: 1).</param>
    /// <param name="itensPorPagina">Número de itens por página (padrão: 20).</param>
    /// <returns>Retorna as inscrições encontradas.</returns>
    /// <response code="200">Retorna as inscrições encontradas.</response>
    /// <response code="400">Se os parâmetros de paginação forem inválidos.</response>
    /// <response code="404">Se nenhuma inscrição for encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno no servidor.</response>
    [HttpGet("Todos")]
    [ProducesResponseType(typeof(IEnumerable<InscricaoResponseDto>), StatusCodes.Status200OK)]
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
                nameof(InscricaoController),
                nameof(ObterTodosAsync),
               $"Recebeu os parâmetros: pagina={pagina}, itensPorPagina={itensPorPagina}"
            );

            var response = await _inscricaoServico.ObterTodosAsync(pagina, itensPorPagina);

            _logger.CustomFormatLog(
                LogEventLevel.Information,
                nameof(InscricaoController),
                nameof(ObterTodosAsync),
                $"Completou a execução. Status: {response.Status}. Tempo de execução: {stopwatch.ElapsedMilliseconds} ms"
            );

            return response.Status switch
            {
                ECustomResultStatus.Success => ResponseOk(response.Data?.Select(InscricaoResponseDto.FromEntity)),
                ECustomResultStatus.EntityNotFound => ResponseNotFound(response.Message),
                ECustomResultStatus.HasError or ECustomResultStatus.EntityHasError or ECustomResultStatus.HasValidation => HandleErrors(response),
                _ => ResponseInternalServerError("Ocorreu um erro interno ao processar a solicitação.")
            };
        }
        catch (Exception ex)
        {
            _logger.CustomFormatLog(
                LogEventLevel.Error,
                nameof(InscricaoController),
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
                nameof(InscricaoController),
                nameof(ObterTodosAsync),
                $"Tempo total de execução: {stopwatch.ElapsedMilliseconds} ms"
            );
        }
    }
}