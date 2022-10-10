using Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder opt)
    {
        opt.EnableSensitiveDataLogging();
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))).ToList().ForEach(
            property => property.SetColumnType("varchar(100)")
        );

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).ToList().ForEach(
            relationship => relationship.DeleteBehavior = DeleteBehavior.NoAction
        );        

        //base.OnModelCreating(modelBuilder);
    } 
}

//dotnet ef migrations add Application -c ApplicationDbContext -o Data/Migrations/Application
//dotnet ef database update -c ApplicationDbContext