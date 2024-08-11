namespace CRMEducacional.Core.Interfaces.Services;

public interface ILeadServico
{
    Task<CustomResult<Lead>> AdicionarAsync(Lead lead);

    Task<CustomResult<Lead>> AtualizarAsync(Lead lead);

    Task<CustomResult<Lead>> ObterPorCPFAsync(string cpf);

    Task<CustomResult<Lead>> ObterPorIdAsync(int leadId);

    Task<CustomResult<IEnumerable<Lead>>> ObterTodosAsync(int pagina, int tamanhoPagina);

    Task<CustomResult<Lead>> RemoverAsync(int leadId);
}