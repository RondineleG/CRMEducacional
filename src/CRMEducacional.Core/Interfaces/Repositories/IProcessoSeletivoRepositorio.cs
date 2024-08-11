namespace CRMEducacional.Core.Interfaces.Repositories;

public interface IProcessoSeletivoRepositorio
{
    Task<CustomResult<ProcessoSeletivo>> AdicionarAsync(ProcessoSeletivo processoSeletivo);

    Task<CustomResult<ProcessoSeletivo>> AtualizarAsync(ProcessoSeletivo processoSeletivo);

    Task<CustomResult<ProcessoSeletivo>> ObterPorIdAsync(int processoSeletivoId);

    Task<CustomResult<IEnumerable<ProcessoSeletivo>>> ObterTodosAsync(int pagina, int tamanhoPagina);

    Task<CustomResult<ProcessoSeletivo>> RemoverAsync(int processoSeletivoId);
}