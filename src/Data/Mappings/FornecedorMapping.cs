using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(p => p.Documento)
            .IsRequired()
            .HasColumnType("varchar(14)");

        builder.HasOne(p => p.Endereco)
            .WithOne(n => n.Fornecedor);

        builder.HasMany(p => p.Produtos)
            .WithOne(n => n.Fornecedor)
            .HasForeignKey(n => n.FornecedorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ToTable("Fornecedores");
    }
}
