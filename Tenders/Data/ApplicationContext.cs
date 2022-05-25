using Microsoft.EntityFrameworkCore;
using Tenders.Models;

namespace Tenders.Data;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Tender> Tenders => Set<Tender>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Proposition> Propositions => Set<Proposition>();

    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Company>()
            .HasMany(c => c.Propositions)
            .WithOne(p => p.Company)
            .OnDelete(DeleteBehavior.Cascade);

        // builder.Entity<Company>().HasMany(c => c.WonTenders).WithOne(t => t.CompanyExecutor)
        //     .OnDelete(DeleteBehavior.SetNull);
        
        builder.Entity<Tender>()
            .HasMany(t => t.Propositions)
            .WithOne(c => c.Tender)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Tender>().HasOne(t => t.CompanyOrganizer).WithMany(u => u.Tenders);
        builder.Entity<Tender>().HasOne(t => t.CompanyExecutor).WithMany(u => u.WonTenders);
    }
}