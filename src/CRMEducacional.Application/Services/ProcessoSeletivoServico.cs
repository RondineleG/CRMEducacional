using CRMEducacional.Core.Exceptions;

namespace CRMEducacional.Application.Services;

public class ProcessoSeletivoServico : IProcessoSeletivoServico
{
    public ProcessoSeletivoServico(IProcessoSeletivoRepositorio processoSeletivoRepositorio, IValidacaoService validacaoService)
    {
        _processoSeletivoRepositorio = processoSeletivoRepositorio;
        _validacaoService = validacaoService;
    }

    private readonly IProcessoSeletivoRepositorio _processoSeletivoRepositorio;

    private readonly IValidacaoService _validacaoService;

    public async Task<CustomResult<ProcessoSeletivo>> AdicionarAsync(ProcessoSeletivo processoSeletivo)
    {
        return await ExecutarOperacaoAsync(processoSeletivo, _processoSeletivoRepositorio.AdicionarAsync);
    }

    public async Task<CustomResult<ProcessoSeletivo>> AtualizarAsync(ProcessoSeletivo processoSeletivo)
    {
        return await ExecutarOperacaoAsync(processoSeletivo, _processoSeletivoRepositorio.AtualizarAsync);
    }

    public async Task<CustomResult<ProcessoSeletivo>> ObterPorIdAsync(int processoSeletivoId)
    {
        return await ExecutarOperacaoAsync(() => _processoSeletivoRepositorio.ObterPorIdAsync(processoSeletivoId));
    }

    public async Task<CustomResult<IEnumerable<ProcessoSeletivo>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        return await ExecutarOperacaoAsync(() => _processoSeletivoRepositorio.ObterTodosAsync(pagina, tamanhoPagina));
    }

    public async Task<CustomResult<ProcessoSeletivo>> RemoverAsync(int processoSeletivoId)
    {
        return await ExecutarOperacaoAsync(() => _processoSeletivoRepositorio.RemoverAsync(processoSeletivoId));
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

    private async Task<CustomResult<ProcessoSeletivo>> ExecutarOperacaoAsync(ProcessoSeletivo processoSeletivo, Func<ProcessoSeletivo, Task<CustomResult<ProcessoSeletivo>>> operacao)
    {
        var resultadoValidacao = ValidarProcessoSeletivo(processoSeletivo);
        if (!resultadoValidacao.IsValid)
        {
            var resultado = new CustomResult<ProcessoSeletivo>();
            foreach (var error in resultadoValidacao.Errors)
            {
                resultado.Status = ECustomResultStatus.EntityHasError;
                resultado.GeneralErrors.Add(error);
            }
            return resultado;
        }

        return await ExecutarOperacaoAsync(() => operacao(processoSeletivo));
    }

    private CustomValidationResult ValidarProcessoSeletivo(ProcessoSeletivo processoSeletivo)
    {
        var resultadoValidacao = new CustomValidationResult();
        resultadoValidacao.Merge(_validacaoService.ValidarProcessoSeletivo(processoSeletivo));
        return resultadoValidacao;
    }
}