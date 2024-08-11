namespace CRMEducacional.Core.Interfaces.Services;

public interface IOfertaServico
{
    Task<CustomResult<Oferta>> AdicionarAsync(Oferta oferta);

    Task<CustomResult<Oferta>> AtualizarAsync(Oferta oferta);

    Task<CustomResult<Oferta>> ObterPorIdAsync(int ofertaId);

    Task<CustomResult<IEnumerable<Oferta>>> ObterTodosAsync(int pagina, int tamanhoPagina);

    Task<CustomResult<Oferta>> RemoverAsync(int ofertaId);
}