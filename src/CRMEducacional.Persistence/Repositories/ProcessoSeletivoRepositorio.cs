namespace CRMEducacional.Persistence.Repositories;

public class ProcessoSeletivoRepositorio : IProcessoSeletivoRepositorio
{
    public ProcessoSeletivoRepositorio(EntityFrameworkDataContext context)
    {
        _context = context;
    }

    private readonly EntityFrameworkDataContext _context;

    public async Task<CustomResult<ProcessoSeletivo>> AdicionarAsync(ProcessoSeletivo processoSeletivo)
    {
        try
        {
            _context.ProcessosSeletivos.Add(processoSeletivo);
            await _context.SaveChangesAsync();
            return CustomResult<ProcessoSeletivo>.Success(processoSeletivo);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.UnexpectedError(nameof(AdicionarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<ProcessoSeletivo>> AtualizarAsync(ProcessoSeletivo processoSeletivo)
    {
        try
        {
            var existingProcessoSeletivo = await _context.Leads.FindAsync(processoSeletivo.Id);
            if (existingProcessoSeletivo == null)
            {
                return CustomResult<ProcessoSeletivo>.WithError($"ProcessoSeletivo com ID {processoSeletivo.Id} não encontrado.");
            }
            _context.Entry(existingProcessoSeletivo).State = EntityState.Detached;
            _context.Entry(processoSeletivo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CustomResult<ProcessoSeletivo>.Success(processoSeletivo);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.UnexpectedError(nameof(AtualizarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<ProcessoSeletivo>> ObterPorIdAsync(int processoSeletivoId)
    {
        try
        {
            var processoSeletivo = await _context.ProcessosSeletivos.FindAsync(processoSeletivoId);
            return processoSeletivo != null ?
               CustomResult<ProcessoSeletivo>.Success(processoSeletivo) :
               CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.EntityNotFound(nameof(ProcessoSeletivo), processoSeletivoId));
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterPorIdAsync), exception.Message));
        }
    }

    public async Task<CustomResult<IEnumerable<ProcessoSeletivo>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        try
        {
            var processosSeletivos = await _context.ProcessosSeletivos
                .OrderBy(c => c.Id)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return CustomResult<IEnumerable<ProcessoSeletivo>>.Success(processosSeletivos);
        }
        catch (Exception ex)
        {
            return CustomResult<IEnumerable<ProcessoSeletivo>>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterTodosAsync), ex.Message));
        }
    }

    public async Task<CustomResult<ProcessoSeletivo>> RemoverAsync(int processoSeletivoId)
    {
        try
        {
            var processoSeletivo = await _context.ProcessosSeletivos.FindAsync(processoSeletivoId);
            if (processoSeletivo == null)
            {
                return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.EntityNotFound(nameof(ProcessoSeletivo), processoSeletivoId));
            }
            _context.ProcessosSeletivos.Remove(processoSeletivo);
            await _context.SaveChangesAsync();
            return CustomResult<ProcessoSeletivo>.Success(processoSeletivo);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<ProcessoSeletivo>.WithError(CustomMessageHandler.UnexpectedError(nameof(RemoverAsync), exception.Message));
        }
    }
}