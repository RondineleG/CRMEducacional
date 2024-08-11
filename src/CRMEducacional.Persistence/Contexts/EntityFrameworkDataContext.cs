namespace CRMEducacional.Persistence.Contexts;

public sealed class EntityFrameworkDataContext(DbContextOptions<EntityFrameworkDataContext> options) : DbContext(options)
{
    public DbSet<Inscricao> Inscricoes { get; set; }

    public DbSet<Lead> Leads { get; set; }

    public DbSet<Oferta> Ofertas { get; set; }

    public DbSet<ProcessoSeletivo> ProcessosSeletivos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityFrameworkDataContext).Assembly);
    }
}