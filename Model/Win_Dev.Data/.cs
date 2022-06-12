using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_Dev.Data
{
    class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal WinTaskContext _context;
        internal DbSet<TEntity> _dbSet;

        public BaseRepository(WinTaskContext context)
        {
            this._context = context;
            this._dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Searches the database for an entry specified by ID and Dto class, or returns null
        /// </summary>
        public virtual TEntity FindByID(Guid id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(Guid id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public IEnumerable<TEntity> FindAll()
        {
            IQueryable<TEntity> query = _dbSet;
            return query;
        }

        public void Dispose()
        {

        }
    }

}

