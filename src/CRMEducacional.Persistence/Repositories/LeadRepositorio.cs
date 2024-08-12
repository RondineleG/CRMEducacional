namespace CRMEducacional.Persistence.Repositories;

public class LeadRepositorio : ILeadRepositorio
{
    public LeadRepositorio(EntityFrameworkDataContext context)
    {
        _context = context;
    }

    private readonly EntityFrameworkDataContext _context;

    public async Task<CustomResult<Lead>> AdicionarAsync(Lead lead)
    {
        try
        {
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return CustomResult<Lead>.Success(lead);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.UnexpectedError(nameof(AdicionarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<Lead>> AtualizarAsync(Lead lead)
    {
        if (lead == null)
        {
            return CustomResult<Lead>.WithError("Lead não pode ser nulo.");
        }

        try
        {
            var existingLead = await _context.Leads.FindAsync(lead.Id);
            if (existingLead == null)
            {
                return CustomResult<Lead>.WithError($"Lead com ID {lead.Id} não encontrado.");
            }
            _context.Entry(existingLead).State = EntityState.Detached;
            _context.Entry(lead).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CustomResult<Lead>.Success(lead);
        }
        catch (DbUpdateConcurrencyException)
        {
            return CustomResult<Lead>.WithError("O lead foi modificado. Por favor, tente novamente.");
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.UnexpectedError(nameof(AtualizarAsync), exception.Message));
        }
    }

    public async Task<CustomResult<Lead>> ObterPorCPFAsync(string cpf)
    {
        try
        {
            if (string.IsNullOrEmpty(cpf))
            {
                return CustomResult<Lead>.WithError(CustomMessageHandler.InvalidParameter("CPF"));
            }

            var lead = await _context.Leads.FirstOrDefaultAsync(i => i.CPF == cpf);

            return lead != null
                ? CustomResult<Lead>.Success(lead)
                : CustomResult<Lead>.WithError(CustomMessageHandler.EntityNotFound("Lead", cpf));
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterPorCPFAsync), exception.Message));
        }
    }

    public async Task<CustomResult<Lead>> ObterPorIdAsync(int leadId)
    {
        try
        {
            var lead = await _context.Leads.FindAsync(leadId);
            return lead != null ?
               CustomResult<Lead>.Success(lead) :
               CustomResult<Lead>.WithError(CustomMessageHandler.EntityNotFound("Lead", leadId));
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterPorIdAsync), exception.Message));
        }
    }

    public async Task<CustomResult<IEnumerable<Lead>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        try
        {
            var leads = await _context.Leads
                .OrderBy(c => c.Id)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return CustomResult<IEnumerable<Lead>>.Success(leads);
        }
        catch (Exception ex)
        {
            return CustomResult<IEnumerable<Lead>>.WithError(CustomMessageHandler.UnexpectedError(nameof(ObterTodosAsync), ex.Message));
        }
    }

    public async Task<CustomResult<Lead>> RemoverAsync(int leadId)
    {
        try
        {
            var lead = await _context.Leads.FindAsync(leadId);
            if (lead == null)
            {
                return CustomResult<Lead>.WithError(CustomMessageHandler.EntityNotFound(nameof(Lead), leadId));
            }
            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return CustomResult<Lead>.Success(lead);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.DbErrorMessage(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Lead>.WithError(CustomMessageHandler.UnexpectedError(nameof(RemoverAsync), exception.Message));
        }
    }
}