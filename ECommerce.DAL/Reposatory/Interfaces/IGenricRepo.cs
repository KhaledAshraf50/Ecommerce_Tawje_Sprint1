using System.Linq.Expressions;

namespace ECommerce_Tawj.Reposatory.Interfaces
{
    public interface IGenricRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate);
        Task<T?> GetByIdAsync(int id);
        void Add(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        // Additional methods for filtering, sorting, and pagination
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    }
}
