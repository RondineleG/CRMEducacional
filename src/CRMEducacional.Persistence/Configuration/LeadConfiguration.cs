using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMEducacional.Persistence.Configuration;

public class LeadConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(l => l.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(l => l.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(l => l.CPF)
            .IsRequired()
            .HasMaxLength(11);

        builder.HasIndex(l => l.CPF)
            .IsUnique();

        builder.HasMany(l => l.Inscricoes)
            .WithOne(i => i.Lead)
            .HasForeignKey(i => i.LeadId);
    }
}