using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMEducacional.Persistence.Configuration;

public class InscricaoConfiguration : IEntityTypeConfiguration<Inscricao>
{
    public void Configure(EntityTypeBuilder<Inscricao> builder)
    {
        builder.ToTable("Inscricoes");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.NumeroInscricao)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.Data)
            .IsRequired();

        builder.Property(i => i.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(i => i.Lead)
            .WithMany(l => l.Inscricoes)
            .HasForeignKey(i => i.LeadId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Oferta)
            .WithMany(o => o.Inscricoes)
            .HasForeignKey(i => i.OfertaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.ProcessoSeletivo)
            .WithMany(p => p.Inscricoes)
            .HasForeignKey(i => i.ProcessoSeletivoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}