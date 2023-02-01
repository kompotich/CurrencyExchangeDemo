using CurrencyExchange.Crawler.Database;
using CurrencyExchange.Crawler.Database.Entities;
using CurrencyExchange.Crawler.Database.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Crawler.Database;

public class SettingRepository : ISettingRepository
{
    private readonly CurrencyExchangeContext _context;

    public SettingRepository(CurrencyExchangeContext context)
    {
        _context = context;
    }

    public IQueryable<Setting> GetAll() =>
        _context.Settings.AsNoTracking();

    public IQueryable<Setting> Find(Expression<Func<Setting, bool>> expression) =>
        _context.Settings.AsNoTracking().Where(expression);

    public async Task AddAsync(Setting setting)
    {
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();
    }

    public void Add(Setting setting)
    {
        _context.Settings.Add(setting);
        _context.SaveChanges();
    }

    public async Task UpdateAsync(Setting setting)
    {
        _context.Settings.Update(setting);
        await _context.SaveChangesAsync();
    }

    public void Update(Setting setting)
    {
        _context.Settings.Update(setting);
        _context.SaveChanges();
    }

    public async Task<Setting?> GetSettingAsync(string settingName) =>
        await _context.Settings.SingleOrDefaultAsync(x => x.Name == settingName);

    public async Task<string?> GetSettingValueAsync(string settingName) =>
        (await _context.Settings.SingleOrDefaultAsync(x => x.Name == settingName))?.Value;
    
}
