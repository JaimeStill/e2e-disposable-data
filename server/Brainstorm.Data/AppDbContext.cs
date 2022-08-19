using Brainstorm.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brainstorm.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Note> Notes { get; set; }
    public DbSet<Topic> Topics { get; set; }

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