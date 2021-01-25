using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyFileSystem.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly FileSystemDbContext _context = null;
        private readonly DbSet<T> table = null;
     
        public BaseRepository(FileSystemDbContext _context) 
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null, string Includes = null)
        {
            if(predicate != null)
            {
            var query = table.Where(predicate);
                if(Includes !=null)
                {
                    foreach (var str in Includes.Split(","))
                        query = query.Include(str).AsQueryable();
                }
                return await query.ToListAsync();
            }
            return await table.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllIncluded(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] Includes)
        {
            var query = table.Where(predicate);
            foreach (var Include in Includes)
            {
                query = query.Include(Include);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetById(object id)
        { 
            return await table.FindAsync(id);
        }

        public void Add(T obj)
        { 
            table.Add(obj); 
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public T Delete(T existing)
        {
            table.Remove(existing);
            return existing;
        }

        public Task DeleteRange(List<T> entites)
        {
            table.RemoveRange(entites);
            return Task.CompletedTask;
        }
    } 
}
