using System.Linq.Expressions;
using ControleDietaApi.Context;
using ControleDietaApi.Dto;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDietaApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }


    public async Task<User?> GetByIdAsync(Expression<Func<User, bool>> predicate)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        return user;
    }
}