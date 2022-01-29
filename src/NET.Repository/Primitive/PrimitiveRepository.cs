using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace NET.Repository.Primitive
{
    public class PrimitiveRepository<T> : IPrimitiveRepository<T>
        where T : class
    {
        /// <summary>
        ///     Internal DbContext instance for changes manipulation
        /// </summary>
        private readonly DbContext _context;
        /// <summary>
        ///     Internal DbSet instance for data manipulation
        /// </summary>
        private readonly DbSet<T> _set;

        public PrimitiveRepository(DbContext context, DbSet<T> set)
        {
            _context = context;
            _set = set;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = default,
            Expression<Func<T, object>>[] includeProperties = default,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = default)
        {
            IQueryable<T> query = _set;

            if (filter != default)
            {
                query = query.Where(filter);
            }

            if (includeProperties != default)
            {
                query = includeProperties.Aggregate(
                    query,
                    (current, includeProperty) => current.Include(includeProperty));
            }

            if (orderBy != default)
            {
                query = orderBy.Invoke(query).AsQueryable();
            }

            return query.ToList();
        }

        public T GetById(object id)
        {
            return _set.Find(id);
        }

        public void Insert(T entity)
        {
            _set.Add(entity);
        }

        public void Delete(object id)
        {
            T entityToDelete = _set.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _set.Attach(entityToDelete);
            }
            _set.Remove(entityToDelete);
        }

        public void Update(T entityToUpdate)
        {
            _set.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}