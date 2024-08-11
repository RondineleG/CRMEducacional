using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMEducacional.Persistence.Configuration;

public class ProcessoSeletivoConfiguration : IEntityTypeConfiguration<ProcessoSeletivo>
{
    public void Configure(EntityTypeBuilder<ProcessoSeletivo> builder)
    {
        builder.ToTable("ProcessosSeletivos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.DataInicio)
            .IsRequired();

        builder.Property(p => p.DataTermino)
            .IsRequired();

        builder.HasMany(p => p.Inscricoes)
            .WithOne(i => i.ProcessoSeletivo)
            .HasForeignKey(i => i.ProcessoSeletivoId);
    }
}