using CRMEducacional.Core.Exceptions;

namespace CRMEducacional.Application.Services;

public class InscricaoServico : IInscricaoServico
{
    public InscricaoServico(
        IInscricaoRepositorio inscricaoRepositorio,
        ILeadRepositorio leadRepositorio,
        IProcessoSeletivoRepositorio processoSeletivoRepositorio,
        IOfertaRepositorio ofertaRepositorio,
        IValidacaoService validacaoService)
    {
        _inscricaoRepositorio = inscricaoRepositorio;
        _leadRepositorio = leadRepositorio;
        _processoSeletivoRepositorio = processoSeletivoRepositorio;
        _ofertaRepositorio = ofertaRepositorio;
        _validacaoService = validacaoService;
    }

    private readonly IInscricaoRepositorio _inscricaoRepositorio;

    private readonly ILeadRepositorio _leadRepositorio;

    private readonly IOfertaRepositorio _ofertaRepositorio;

    private readonly IProcessoSeletivoRepositorio _processoSeletivoRepositorio;

    private readonly IValidacaoService _validacaoService;

    public async Task<CustomResult<Inscricao>> AdicionarAsync(Inscricao inscricao)
    {
        return await ExecutarOperacaoAsync(inscricao, _inscricaoRepositorio.AdicionarAsync);
    }

    public async Task<CustomResult<Inscricao>> AtualizarAsync(Inscricao inscricao)
    {
        return await ExecutarOperacaoAsync(inscricao, _inscricaoRepositorio.AtualizarAsync);
    }

    public async Task<CustomResult<IEnumerable<Inscricao>>> ObterPorCPFAsync(string cpf)
    {
        return await ExecutarOperacaoAsync(() => _inscricaoRepositorio.ObterPorCPFAsync(cpf));
    }

    public async Task<CustomResult<Inscricao>> ObterPorIdAsync(int inscricaoId)
    {
        return await ExecutarOperacaoAsync(() => _inscricaoRepositorio.ObterPorIdAsync(inscricaoId));
    }

    public async Task<CustomResult<IEnumerable<Inscricao>>> ObterPorOfertaAsync(int ofertaId)
    {
        return await ExecutarOperacaoAsync(() => _inscricaoRepositorio.ObterPorOfertaAsync(ofertaId));
    }

    public async Task<CustomResult<IEnumerable<Inscricao>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        return await ExecutarOperacaoAsync(() => _inscricaoRepositorio.ObterTodosAsync(pagina, tamanhoPagina));
    }

    public async Task<CustomResult<Inscricao>> RemoverAsync(int inscricaoId)
    {
        return await ExecutarOperacaoAsync(() => _inscricaoRepositorio.RemoverAsync(inscricaoId));
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

    private async Task<CustomResult<Inscricao>> ExecutarOperacaoAsync(Inscricao inscricao, Func<Inscricao, Task<CustomResult<Inscricao>>> operacao)
    {
        var resultadoValidacao = await ValidarInscricaoAsync(inscricao);
        if (!resultadoValidacao.IsValid)
        {
            var resultado = new CustomResult<Inscricao>();
            foreach (var error in resultadoValidacao.Errors)
            {
                resultado.Status = ECustomResultStatus.EntityHasError;
                resultado.GeneralErrors.Add(error);
            }
            return resultado;
        }

        return await ExecutarOperacaoAsync(() => operacao(inscricao));
    }

    private async Task<CustomValidationResult> ValidarInscricaoAsync(Inscricao inscricao)
    {
        var resultadoValidacao = new CustomValidationResult();

        await resultadoValidacao.ValidarEntidadeRelacionada(
            inscricao.LeadId,
            _leadRepositorio.ObterPorIdAsync,
            "Lead não encontrado.",
            nameof(inscricao.LeadId),
            inscricao.SetLead,
            resultadoValidacao);

        await resultadoValidacao.ValidarEntidadeRelacionada(
            inscricao.ProcessoSeletivoId,
            _processoSeletivoRepositorio.ObterPorIdAsync,
            "Processo Seletivo não encontrado.",
            nameof(inscricao.ProcessoSeletivoId),
            inscricao.SetProcessoSeletivo,
            resultadoValidacao);

        await resultadoValidacao.ValidarEntidadeRelacionada(
            inscricao.OfertaId,
            _ofertaRepositorio.ObterPorIdAsync,
            "Oferta não encontrada.",
            nameof(inscricao.OfertaId),
            inscricao.SetOferta,
            resultadoValidacao);

        resultadoValidacao.Merge(_validacaoService.ValidarInscricao(inscricao));
        return resultadoValidacao;
    }
}