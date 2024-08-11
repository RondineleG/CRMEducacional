using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMEducacional.Persistence.Configuration;

public class OfertaConfiguration : IEntityTypeConfiguration<Oferta>
{
    public void Configure(EntityTypeBuilder<Oferta> builder)
    {
        builder.ToTable("Ofertas");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(o => o.Descricao)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(o => o.VagasDisponiveis)
            .IsRequired();

        builder.HasMany(o => o.Inscricoes)
            .WithOne(i => i.Oferta)
            .HasForeignKey(i => i.OfertaId);
    }
}