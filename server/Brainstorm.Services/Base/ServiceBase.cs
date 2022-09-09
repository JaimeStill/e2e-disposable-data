using Brainstorm.Data;
using Brainstorm.Extensions;
using Brainstorm.Models;
using Brainstorm.Models.Interfaces;
using Brainstorm.Models.Query;
using Microsoft.EntityFrameworkCore;

namespace Brainstorm.Services;
public class ServiceBase<T> : IService<T> where T : EntityBase
{
    protected AppDbContext db;
    protected DbSet<T> set;
    protected IQueryable<T> query;

    public ServiceBase(AppDbContext db)
    {
        this.db = db;
        set = db.Set<T>();
        query = SetGraph(set);
    }

    protected virtual Func<IQueryable<T>, string, IQueryable<T>> Search => (values, term) => values;

    protected virtual IQueryable<T> SetGraph(DbSet<T> data) => data;

    protected virtual async Task<QueryResult<T>> Query(
        IQueryable<T> queryable,
        QueryParams queryParams,
        Func<IQueryable<T>, string, IQueryable<T>> search
    )
    {
        var container = new QueryContainer<T>(
            queryable,
            queryParams
        );

        return await container.Query((data, s) =>
            data.SetupSearch(s, search));
    }

    protected virtual async Task<T> Add(T entity)
    {
        try
        {
            await set.AddAsync(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new ServiceException<T>("Add", ex);
        }
    }

    protected virtual async Task<T> Update(T entity)
    {
        try
        {
            set.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new ServiceException<T>("Update", ex);
        }
    }
    
    public virtual async Task<QueryResult<T>> Query(QueryParams queryParams) =>
        await Query(
            query, queryParams, Search
        );

    public virtual async Task<T> GetById(int id) =>
        await query.FirstOrDefaultAsync(x => x.Id == id);

    public virtual async Task<T> GetByUrl(string url) =>
        await query.FirstOrDefaultAsync(x => x.Url.ToLower() == url.ToLower());

    public virtual Task<bool> Validate(T entity) => Task.FromResult(true);

    public virtual async Task<T> AddOrUpdate(T entity)
    {
        try
        {
            if (await Validate(entity))
                return entity.Id > 0
                    ? await Update(entity)
                    : await Add(entity);

            return null;
        }
        catch(Exception ex)
        {
            throw new ServiceException<T>("AddOrUpdate", ex);
        }
    }

    public virtual async Task<bool> Remove(T entity)
    {
        try
        {
            set.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            throw new ServiceException<T>("Remove", ex);
        }     
    }
}