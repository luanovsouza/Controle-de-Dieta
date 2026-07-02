using System.Linq.Expressions;
using ControleDietaApi.Dto;
using ControleDietaApi.Models;

namespace ControleDietaApi.Repositories.Interfaces;

public interface IUserRepository 
{
    Task<User> GetByIdAsync(Expression<Func<User, bool>> predicate);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
}