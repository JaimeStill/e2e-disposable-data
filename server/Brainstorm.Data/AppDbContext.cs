using Brainstorm.Models;
using Brainstorm.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brainstorm.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Note> Notes { get; set; }
    public DbSet<Topic> Topics { get; set; }

    private IEnumerable<EntityBase> Changes =>
        ChangeTracker
            .Entries()
            .Where(x => x.Entity is EntityBase)
            .Select(x => x.Entity)
            .Cast<EntityBase>();

    private void Settle()
    {
        foreach (var change in Changes)
            change.Settle();
    }

    public override int SaveChanges()
    {
        Settle();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        Settle();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Settle();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        Settle();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Model
            .GetEntityTypes()
            .Where(x => x.BaseType == null)
            .ToList()
            .ForEach(x =>
            {
                modelBuilder
                    .Entity(x.Name)
                    .ToTable(x.Name.Split('.').Last());
            });
    }
}