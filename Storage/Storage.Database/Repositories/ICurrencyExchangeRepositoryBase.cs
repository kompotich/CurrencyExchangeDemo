using CurrencyExchange.Storage.Database.Entities.Models;
using System.Linq.Expressions;

namespace CurrencyExchange.Storage.Database.Repositories;

public interface ICurrencyExchangeRepositoryBase<T> 
    where T : CurrencyExchangeEntityBase
{
    T? GetById(int id);
    IQueryable<T> GetAll();
    IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void Delete(int id);
    void DeleteAll();
}
