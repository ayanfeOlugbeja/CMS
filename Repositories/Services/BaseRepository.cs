using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Repositories.Services
{
    public class BaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>() .Where(x => x.IsActive) .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>() .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.IsActive = true; 
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> UpdateAsync(int id, Action<T> updateAction)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null || !entity.IsActive) return null;

            updateAction(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null || !entity.IsActive) return false;

            entity.IsActive = false; 
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
