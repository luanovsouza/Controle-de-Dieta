using ControleDietaApi.Context;
using ControleDietaApi.Models;
using ControleDietaApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleDietaApi.Repositories;

public class MealUserRepository : IMealsUserRepository
{
    private readonly AppDbContext _context;
    
    public MealUserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<MeatGoal>> GetByUserIdAsync(int userId)
    {
        var existeUser = await _context.Users.AnyAsync(x => x.Id == userId);

        if (!existeUser)
            throw new Exception("Usuario nao encontrado!");

        return await _context.MeatGoals
            .Where(x => x.UserId == userId).ToListAsync();
    }
}