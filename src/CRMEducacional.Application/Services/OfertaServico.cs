using CRMEducacional.Core.Exceptions;

namespace CRMEducacional.Application.Services;

public class OfertaServico : IOfertaServico
{
    public OfertaServico(IOfertaRepositorio ofertaRepositorio, IValidacaoService validacaoService)
    {
        _ofertaRepositorio = ofertaRepositorio;
        _validacaoService = validacaoService;
    }

    private readonly IOfertaRepositorio _ofertaRepositorio;

    private readonly IValidacaoService _validacaoService;

    public async Task<CustomResult<Oferta>> AdicionarAsync(Oferta oferta)
    {
        return await ExecutarOperacaoAsync(oferta, _ofertaRepositorio.AdicionarAsync);
    }

    public async Task<CustomResult<Oferta>> AtualizarAsync(Oferta oferta)
    {
        return await ExecutarOperacaoAsync(oferta, _ofertaRepositorio.AtualizarAsync);
    }

    public async Task<CustomResult<Oferta>> ObterPorIdAsync(int ofertaId)
    {
        return await ExecutarOperacaoAsync(() => _ofertaRepositorio.ObterPorIdAsync(ofertaId));
    }

    public async Task<CustomResult<IEnumerable<Oferta>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        return await ExecutarOperacaoAsync(() => _ofertaRepositorio.ObterTodosAsync(pagina, tamanhoPagina));
    }

    public async Task<CustomResult<Oferta>> RemoverAsync(int ofertaId)
    {
        return await ExecutarOperacaoAsync(() => _ofertaRepositorio.RemoverAsync(ofertaId));
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

    private async Task<CustomResult<Oferta>> ExecutarOperacaoAsync(Oferta oferta, Func<Oferta, Task<CustomResult<Oferta>>> operacao)
    {
        var resultadoValidacao = ValidarOferta(oferta);
        if (!resultadoValidacao.IsValid)
        {
            var resultado = new CustomResult<Oferta>();
            foreach (var error in resultadoValidacao.Errors)
            {
                resultado.Status = ECustomResultStatus.EntityHasError;
                resultado.GeneralErrors.Add(error);
            }
            return resultado;
        }

        return await ExecutarOperacaoAsync(() => operacao(oferta));
    }

    private CustomValidationResult ValidarOferta(Oferta oferta)
    {
        var resultadoValidacao = new CustomValidationResult();
        resultadoValidacao.Merge(_validacaoService.ValidarOferta(oferta));
        return resultadoValidacao;
    }
}