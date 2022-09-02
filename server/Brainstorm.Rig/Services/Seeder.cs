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

    public event EventHandler<DataSeededEventArgs> DataSeeded;

    void OnDataSeeded(string method) =>
        DataSeeded.Invoke(this, new() { Message = $"{method} data successfully saved"});

    public async Task<T> Seed<T>(T entity) where T : EntityBase
    {
        await Context.Set<T>()
            .AddAsync(entity);

        await Context.SaveChangesAsync();

        OnDataSeeded($"Seed<{typeof(T)}>");

        return entity;
    }

    public async Task<List<T>> SeedMany<T>(List<T> entities) where T : EntityBase
    {
        await Context.Set<T>()
            .AddRangeAsync(entities);

        await Context.SaveChangesAsync();

        OnDataSeeded($"SeedMany<{typeof(T)}>");

        return entities;
    }
}

public class DataSeededEventArgs : EventArgs
{
    public string Message { get; set; }
}