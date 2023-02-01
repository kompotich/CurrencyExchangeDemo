using CurrencyExchange.Crawler.Database.Entities.Models;
using System.Linq.Expressions;

namespace CurrencyExchange.Crawler.Database;

public interface ISettingRepository
{
    IQueryable<Setting> GetAll();
    
    IQueryable<Setting> Find(Expression<Func<Setting, bool>> expression);
    
    Task AddAsync(Setting setting);

    void Add(Setting setting);

    Task UpdateAsync(Setting setting);

    void Update(Setting setting);

    Task<Setting?> GetSettingAsync(string settingName);

    Task<string?> GetSettingValueAsync(string settingName);
}
