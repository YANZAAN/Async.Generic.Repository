using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using NET.Repository.Full.Logic;
using NET.Repository.Full.Utility;

namespace NET.Repository.Full
{
    public interface IFullRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> Get(Specification<T> condition,
            Expression<Func<T, object>>[] includeProperties,
            PaginationContext pageContext,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        
        T Find(Specification<T> condition,
            Expression<Func<T, object>>[] includeProperties);

        void Insert(T entity);

        void Insert(IEnumerable<T> entitiesToInsert);

        void Delete(object id);

        void Delete(T entityToDelete);

        void Delete(IEnumerable<T> entitiesToDelete);

        void Update(T entityToUpdate);

        void Update(IEnumerable<T> entitiesToUpdate);
    }
}