using CurrencyExchange.Storage.Database.Entities;
using CurrencyExchange.Storage.Database.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CurrencyExchange.Storage.Database.Repositories;

public class CurrencyExchangeRepositoryBase<T> : ICurrencyExchangeRepositoryBase<T>
    where T : CurrencyExchangeEntityBase
{
    private readonly CurrencyExchangeContext _currencyExchangeContext;

    public CurrencyExchangeRepositoryBase(CurrencyExchangeContext currencyExchangeContext)
    {
        _currencyExchangeContext = currencyExchangeContext;
    }

    public T? GetById(int id) => _currencyExchangeContext.Set<T>().SingleOrDefault(x => x.Id == id);

    public IQueryable<T> GetAll() => _currencyExchangeContext.Set<T>().AsNoTracking();

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression) =>
        _currencyExchangeContext.Set<T>().Where(expression).AsNoTracking();

    public void Add(T entity) => _currencyExchangeContext.Set<T>().Add(entity);

    public void AddRange(IEnumerable<T> entities) => _currencyExchangeContext.Set<T>().AddRange(entities);

    public void Update(T entity) => _currencyExchangeContext.Set<T>().Update(entity);

    public void UpdateRange(IEnumerable<T> entities) =>
        _currencyExchangeContext.Set<T>().UpdateRange(entities);

    public void Delete(T entity) => _currencyExchangeContext.Set<T>().Remove(entity);

    public void Delete(int id)
    {
        var entity = GetById(id);

        if (entity != null)
        {
            _currencyExchangeContext.Set<T>().Remove(entity);
        }
    }

    public void DeleteAll() =>
        _currencyExchangeContext.Set<T>().RemoveRange(GetAll());
}
