using CRMEducacional.Core.Exceptions;

namespace CRMEducacional.Application.Services;

public class LeadServico : ILeadServico
{
    public LeadServico(ILeadRepositorio leadRepositorio, IValidacaoService validacaoService)
    {
        _leadRepositorio = leadRepositorio;
        _validacaoService = validacaoService;
    }

    private readonly ILeadRepositorio _leadRepositorio;

    private readonly IValidacaoService _validacaoService;

    public async Task<CustomResult<Lead>> AdicionarAsync(Lead lead)
    {
        return await ExecutarOperacaoAsync(lead, _leadRepositorio.AdicionarAsync);
    }

    public async Task<CustomResult<Lead>> AtualizarAsync(Lead lead)
    {
        return await ExecutarOperacaoAsync(lead, _leadRepositorio.AtualizarAsync);
    }

    public async Task<CustomResult<Lead>> ObterPorCPFAsync(string cpf)
    {
        return await ExecutarOperacaoAsync(() => _leadRepositorio.ObterPorCPFAsync(cpf));
    }

    public async Task<CustomResult<Lead>> ObterPorIdAsync(int leadId)
    {
        return await ExecutarOperacaoAsync(() => _leadRepositorio.ObterPorIdAsync(leadId));
    }

    public async Task<CustomResult<IEnumerable<Lead>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        return await ExecutarOperacaoAsync(() => _leadRepositorio.ObterTodosAsync(pagina, tamanhoPagina));
    }

    public async Task<CustomResult<Lead>> RemoverAsync(int leadId)
    {
        return await ExecutarOperacaoAsync(() => _leadRepositorio.RemoverAsync(leadId));
    }

    private async Task<CustomResult<T>> ExecutarOperacaoAsync<T>(Func<Task<CustomResult<T>>> operacao)
    {
        try
        {
            return await operacao();
        }
        catch (CustomResultException cre)
        {
            return CustomResult<T>.WithError(cre.CustomResult.Message);
        }
        catch (Exception exception)
        {
            return CustomResult<T>.WithError($"Erro ao processar operação: {exception.Message}");
        }
    }

    private async Task<CustomResult<Lead>> ExecutarOperacaoAsync(Lead lead, Func<Lead, Task<CustomResult<Lead>>> operacao)
    {
        var resultadoValidacao = ValidarLead(lead);
        if (!resultadoValidacao.IsValid)
        {
            var resultado = new CustomResult<Lead>();

            foreach (var error in resultadoValidacao.Errors)
            {
                resultado.Status = ECustomResultStatus.EntityHasError;
                resultado.GeneralErrors.Add(error);
            }

            return resultado;
        }

        return await ExecutarOperacaoAsync(() => operacao(lead));
    }

    private CustomValidationResult ValidarLead(Lead lead)
    {
        var resultadoValidacao = new CustomValidationResult();
        resultadoValidacao.Merge(_validacaoService.ValidarLead(lead));
        return resultadoValidacao;
    }
}