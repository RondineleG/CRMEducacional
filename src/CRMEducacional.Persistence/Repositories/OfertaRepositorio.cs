namespace CRMEducacional.Persistence.Repositories;

public class OfertaRepositorio : IOfertaRepositorio
{
    public OfertaRepositorio(EntityFrameworkDataContext context)
    {
        _context = context;
    }

    private readonly EntityFrameworkDataContext _context;

    public async Task<CustomResult<Oferta>> AdicionarAsync(Oferta oferta)
    {
        try
        {
            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();
            return CustomResult<Oferta>.Success(oferta);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Oferta>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Oferta>.WithError(CustomMessageHandler.UnexpectedError(nameof(AdicionarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<Oferta>> AtualizarAsync(Oferta oferta)
    {
        try
        {
            var existingOferta = await _context.Ofertas.FindAsync(oferta.Id);
            if (existingOferta == null)
            {
                return CustomResult<Oferta>.WithError($"Oferta com ID {oferta.Id} não encontrado.");
            }
            _context.Entry(existingOferta).State = EntityState.Detached;
            _context.Entry(oferta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CustomResult<Oferta>.Success(oferta);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Oferta>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Oferta>.WithError(CustomMessageHandler.UnexpectedError(nameof(AtualizarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<Oferta>> ObterPorIdAsync(int ofertaId)
    {
        try
        {
            var oferta = await _context.Ofertas.FindAsync(ofertaId);
            return oferta != null ?
               CustomResult<Oferta>.Success(oferta) :
               CustomResult<Oferta>.WithError(CustomMessageHandler.EntityNotFound(nameof(Oferta), ofertaId));
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Oferta>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Oferta>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterPorIdAsync), exception.Message));
        }
    }

    public async Task<CustomResult<IEnumerable<Oferta>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        try
        {
            var ofertas = await _context.Ofertas
                .OrderBy(c => c.Id)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return CustomResult<IEnumerable<Oferta>>.Success(ofertas);
        }
        catch (Exception ex)
        {
            return CustomResult<IEnumerable<Oferta>>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterTodosAsync), ex.Message));
        }
    }

    public async Task<CustomResult<Oferta>> RemoverAsync(int ofertaId)
    {
        try
        {
            var oferta = await _context.Ofertas.FindAsync(ofertaId);
            if (oferta == null)
            {
                return CustomResult<Oferta>.WithError(CustomMessageHandler.EntityNotFound(nameof(Oferta), ofertaId));
            }
            _context.Ofertas.Remove(oferta);
            await _context.SaveChangesAsync();
            return CustomResult<Oferta>.Success(oferta);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Oferta>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Oferta>.WithError(CustomMessageHandler.UnexpectedError(nameof(RemoverAsync), exception.Message));
        }
    }
}