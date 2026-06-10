using ControleDietaApi.Models;

namespace ControleDietaApi.Repositories.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    Task Commit();
}