using Brainstorm.Models.Query;

namespace Brainstorm.Models.Interfaces;
public interface IService<T> where T : EntityBase
{
    Task<QueryResult<T>> Query(QueryParams queryParams);
    Task<T> GetById(int id);
    Task<T> GetByUrl(string url);
    Task<bool> Validate(T entity);
    Task<T> AddOrUpdate(T entity);
    Task<bool> Remove(T entity);
}