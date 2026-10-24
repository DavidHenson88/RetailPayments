using Microsoft.EntityFrameworkCore;
using RetailPayments.Api.Models;

namespace RetailPayments.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionAudit> TransactionAudits => Set<TransactionAudit>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the composite unique primary key as required by the spec
        modelBuilder.Entity<Transaction>()
            .HasKey(t => new { t.SourceSystem, t.TransactionId });
    }
}