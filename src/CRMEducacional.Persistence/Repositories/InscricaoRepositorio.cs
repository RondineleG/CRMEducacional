namespace CRMEducacional.Persistence.Repositories;

public class InscricaoRepositorio : IInscricaoRepositorio
{
    public InscricaoRepositorio(EntityFrameworkDataContext context)
    { _context = context; }

    private readonly EntityFrameworkDataContext _context;

    public async Task<CustomResult<Inscricao>> AdicionarAsync(Inscricao inscricao)
    {
        try
        {
            _context.Inscricoes.Add(inscricao);
            await _context.SaveChangesAsync();
            return CustomResult<Inscricao>.Success(inscricao);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Inscricao>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Inscricao>.WithError(CustomMessageHandler.UnexpectedError(nameof(AdicionarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<Inscricao>> AtualizarAsync(Inscricao inscricao)
    {
        try
        {
            var existingInscricao = await _context.Inscricoes.FindAsync(inscricao.Id);
            if (existingInscricao == null)
            {
                return CustomResult<Inscricao>.WithError($"Inscricao com ID {inscricao.Id} não encontrado.");
            }
            _context.Entry(existingInscricao).State = EntityState.Detached;
            _context.Entry(inscricao).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CustomResult<Inscricao>.Success(inscricao);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Inscricao>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Inscricao>.WithError(CustomMessageHandler.UnexpectedError(nameof(AtualizarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<IEnumerable<Inscricao>>> ObterPorCPFAsync(string cpf)
    {
        try
        {
            var inscricoes = await _context.Inscricoes.Where(i => i.Lead.CPF == cpf).ToListAsync();
            return CustomResult<IEnumerable<Inscricao>>.Success(inscricoes);
        }
        catch (Exception exception)
        {
            return CustomResult<IEnumerable<Inscricao>>.WithError(CustomMessageHandler.UnexpectedError(nameof(AtualizarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<Inscricao>> ObterPorIdAsync(int inscricaoId)
    {
        try
        {
            var inscricao = await _context.Inscricoes.FindAsync(inscricaoId);
            return inscricao != null ?
               CustomResult<Inscricao>.Success(inscricao) :
               CustomResult<Inscricao>.WithError(CustomMessageHandler.EntityNotFound("Inscricao", inscricaoId));
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Inscricao>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Inscricao>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterPorIdAsync), exception.Message));
        }
    }

    public async Task<CustomResult<IEnumerable<Inscricao>>> ObterPorOfertaAsync(int ofertaId)
    {
        try
        {
            var inscricoes = await _context.Inscricoes.Where(i => i.OfertaId == ofertaId).ToListAsync();
            return inscricoes != null ?
                CustomResult<IEnumerable<Inscricao>>.Success(inscricoes) :
                CustomResult<IEnumerable<Inscricao>>.WithError(CustomMessageHandler.EntityNotFound(nameof(Inscricao), ofertaId));
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<IEnumerable<Inscricao>>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<IEnumerable<Inscricao>>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterPorOfertaAsync), exception.Message));
        }
    }

    public async Task<CustomResult<IEnumerable<Inscricao>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        try
        {
            var inscricoes = await _context.Inscricoes
                .OrderBy(c => c.Id)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return CustomResult<IEnumerable<Inscricao>>.Success(inscricoes);
        }
        catch (Exception exception)
        {
            return CustomResult<IEnumerable<Inscricao>>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterTodosAsync), exception.Message));
        }
    }

    public async Task<CustomResult<Inscricao>> RemoverAsync(int inscricaoId)
    {
        try
        {
            var inscricao = await _context.Inscricoes.FindAsync(inscricaoId);
            if (inscricao == null)
            {
                return CustomResult<Inscricao>.WithError(CustomMessageHandler.EntityNotFound(nameof(Inscricao), inscricaoId));
            }
            _context.Inscricoes.Remove(inscricao);
            await _context.SaveChangesAsync();
            return CustomResult<Inscricao>.Success(inscricao);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Inscricao>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Inscricao>.WithError(CustomMessageHandler.UnexpectedError(nameof(RemoverAsync), exception.Message));
        }
    }
}