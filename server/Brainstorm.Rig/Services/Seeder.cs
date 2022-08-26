using Brainstorm.Data;
using Brainstorm.Models;

namespace Brainstorm.Rig.Services;
public class Seeder
{
    readonly AppDbContext Context;

    public Seeder(AppDbContext context)
    {
        Context = context;
    }

    public async Task<T> Seed<T>(T entity) where T : EntityBase
    {
        await Context.Set<T>()
            .AddAsync(entity);

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<List<T>> SeedMany<T>(List<T> entities) where T : EntityBase
    {
        await Context.Set<T>()
            .AddRangeAsync(entities);

        await Context.SaveChangesAsync();

        return entities;
    }
}