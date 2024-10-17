using GTERP.Interfaces;
using GTERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTERP.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly GTRDBContext _context;
        private DbSet<T> table;

        public GenericRepository(GTRDBContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object Id)
        {
            return await table.FindAsync(Id);
        }

        public async Task InsertAsync(T obj)
        {
            await table.AddAsync(obj);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public async Task DeleteAsync(object Id)
        {
            T exists = await table.FindAsync(Id);
            table.Remove(exists);
        }

    }
}
