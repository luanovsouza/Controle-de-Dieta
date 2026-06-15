using System.Linq.Expressions;
using ControleDietaApi.Context;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDietaApi.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public async Task<T?> GetById(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<T> Create(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
         await _context.SaveChangesAsync();
        return entity;
    }
}