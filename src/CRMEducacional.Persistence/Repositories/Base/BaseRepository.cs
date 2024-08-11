namespace CRMEducacional.Persistence.Repositories.Base;

public abstract class BaseRepository(EntityFrameworkDataContext context)
{
    protected readonly EntityFrameworkDataContext _context = context;
}