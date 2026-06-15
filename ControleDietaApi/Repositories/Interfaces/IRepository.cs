using System.Linq.Expressions;

namespace ControleDietaApi.Repositories.Interfaces;

public interface IRepository <T>
{
    IEnumerable<T> GetAll();
    Task<T> GetById(Expression<Func<T, bool>> predicate);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(T entity); 
}