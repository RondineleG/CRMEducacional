namespace CRMEducacional.Core.Interfaces.Services;

public interface IProcessoSeletivoServico
{
    Task<CustomResult<ProcessoSeletivo>> AdicionarAsync(ProcessoSeletivo processoSeletivo);

    Task<CustomResult<ProcessoSeletivo>> AtualizarAsync(ProcessoSeletivo processoSeletivo);

    Task<CustomResult<ProcessoSeletivo>> ObterPorIdAsync(int processoSeletivoId);

    Task<CustomResult<IEnumerable<ProcessoSeletivo>>> ObterTodosAsync(int pagina, int tamanhoPagina);

    Task<CustomResult<ProcessoSeletivo>> RemoverAsync(int processoSeletivoId);
}