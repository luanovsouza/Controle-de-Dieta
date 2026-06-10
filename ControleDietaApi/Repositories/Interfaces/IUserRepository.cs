using ControleDietaApi.Models;

namespace ControleDietaApi.Repositories.Interfaces;

public interface IUserRepository 
{
    Task<User> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
}