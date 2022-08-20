using Brainstorm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Brainstorm.Data;
public class DbManager : IDisposable
{
    readonly bool destroy;
    public AppDbContext Context { get; private set; }
    public string Connection => Context.Database.GetConnectionString();

    static string GetConnectionString(string env, bool isUnique)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("connections.json")
            .AddEnvironmentVariables()
            .Build();

        string connection = config.GetConnectionString(env);

        if (isUnique)
            connection = $"{connection}-{Guid.NewGuid()}";

        Console.WriteLine($"Connection string: {connection}");

        return connection;
    }

    static AppDbContext GetDbContext(string connection)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connection);

        return new AppDbContext(builder.Options);
    }

    public DbManager(string env = "App", bool destroy = false, bool isUnique = false)
    {
        this.destroy = destroy;
        Context = GetDbContext(GetConnectionString(env, isUnique));
    }

    public async Task<T> SeedGraph<T>(T entity)
        where T : EntityBase
    {
        await Context.Set<T>()
            .AddAsync(entity);

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<List<T>> SeedGraphs<T>(List<T> entities)
        where T : EntityBase
    {
        await Context.Set<T>()
            .AddRangeAsync(entities);

        await Context.SaveChangesAsync();

        return entities;
    }

    public void Initialize()
    {
        if (destroy)
            Context.Database.EnsureDeleted();

        Context.Database.Migrate();
    }

    public async Task<bool> InitializeAsync()
    {
        try
        {
            if (destroy)
                await Context.Database.EnsureDeletedAsync();

            await Context.Database.MigrateAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        Console.WriteLine($"Disposing {Connection}");

        if (destroy)
            Context.Database.EnsureDeleted();

        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
