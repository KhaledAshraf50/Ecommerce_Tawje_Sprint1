using ECommerce_Tawj.Models.Data;
using ECommerce_Tawj.Reposatory.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce_Tawj.Reposatory.Implemention
{
    public class GenricRepo<T> : IGenricRepo<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenricRepo(ApplicationDbContext context) 
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate)
        {
            // تم إضافة AsNoTracking لعدم تتبع البيانات وتحسين أداء القراءة
            if (predicate == null)
                return await _dbSet.AsNoTracking().ToListAsync();
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await  GetByIdAsync(id);
            if (entity != null) 
                _dbSet.Remove(entity);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}
