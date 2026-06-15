using ControleDietaApi.Models;

namespace ControleDietaApi.Repositories.Interfaces;

public interface IMealsUserRepository 
{
    Task<IEnumerable<MeatGoal>> GetByUserIdAsync(int id);
}