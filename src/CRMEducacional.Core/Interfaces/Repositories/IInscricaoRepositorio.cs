namespace CRMEducacional.Core.Interfaces.Repositories;

public interface IInscricaoRepositorio
{
    Task<CustomResult<Inscricao>> AdicionarAsync(Inscricao inscricao);

    Task<CustomResult<Inscricao>> AtualizarAsync(Inscricao inscricao);

    Task<CustomResult<IEnumerable<Inscricao>>> ObterPorCPFAsync(string cpf);

    Task<CustomResult<Inscricao>> ObterPorIdAsync(int inscricaoId);

    Task<CustomResult<IEnumerable<Inscricao>>> ObterPorOfertaAsync(int ofertaId);

    Task<CustomResult<IEnumerable<Inscricao>>> ObterTodosAsync(int pagina, int tamanhoPagina);

    Task<CustomResult<Inscricao>> RemoverAsync(int inscricaoId);
}