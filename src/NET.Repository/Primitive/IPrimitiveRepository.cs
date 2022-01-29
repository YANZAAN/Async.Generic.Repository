using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace NET.Repository.Primitive
{
    public interface IPrimitiveRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> filter,
            Expression<Func<T, object>>[] includeProperties,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        
        T GetById(object id);

        void Insert(T entity);

        void Delete(object id);

        void Delete(T entityToDelete);

        void Update(T entityToUpdate);
    }
}