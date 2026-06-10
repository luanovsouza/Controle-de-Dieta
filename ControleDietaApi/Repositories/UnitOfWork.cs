using ControleDietaApi.Context;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDietaApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public AppDbContext _context;
    public IUserRepository Users { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
    }
    
    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}