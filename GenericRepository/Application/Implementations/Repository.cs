using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using GenericRepository.Application.Interfaces;
using GenericRepository.Helpers.Utility;
using GenericRepository.Logic;

namespace GenericRepository.Application.Implementation
{
    /// <summary>
    /// <para>Asynchronous Generic Repository Implementation</para>
    /// <para>Author: YANZAAN</para>
    /// <para>Source: https://github.com/YANZAAN/net5-async-generic-repository.git</para>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Internal DbContext instance for changes manipulation
        /// </summary>
        private readonly DbContext Context;
        /// <summary>
        /// Internal DbSet instance for data manipulation
        /// </summary>
        private readonly DbSet<T> Set;
        public Repository(DbContext context, DbSet<T> set)
        {
            Context = context;
            Set = set;
        }

        public IAsyncEnumerable<T> Get(Specification<T> condition = null, PaginationContext pageContext = null, params Expression<Func<T, object>>[] includeProperties)
        {
            var complexQuery = Include(includeProperties);
            var filtratedQuery = Filtrate(complexQuery, condition);
            var paginatedQuery = Paginate(filtratedQuery, pageContext);

            return paginatedQuery.ToAsyncEnumerable();
        }

        public async ValueTask<T> Find(Specification<T> condition = null, params Expression<Func<T, object>>[] includeProperties)
        {
            return await Get(condition, PaginationContext.Single, includeProperties).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Makes Include operation on primary dataset
        /// </summary>
        /// <param name="includeProperties">Include resolvers list</param>
        /// <returns>List of IQueryable type with connected tables</returns>
        private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Set.AsNoTracking();

            if (includeProperties.Length == 0)
                return query;

            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        /// <summary>
        /// Filters dataset by specified specification
        /// </summary>
        /// <param name="query">List to filter</param>
        /// <param name="condition">Specification used to filtrate list</param>
        /// <returns>List of IQueryable type with filter applied</returns>
        private IQueryable<T> Filtrate(IQueryable<T> query, Specification<T> condition = null)
        {
            if (condition == null)
                condition = new AbsoluteTrueSpecification<T>();

            return query.Where(condition.ToExpression());
        }
        /// <summary>
        /// Applies page-size reducer to list
        /// </summary>
        /// <param name="query">Elements list</param>
        /// <param name="pageContext">Paging context to apply</param>
        /// <returns>List of IQueryable type and specified page</returns>
        private IQueryable<T> Paginate(IQueryable<T> query, PaginationContext pageContext = null)
        {
            if (pageContext == null)
                pageContext = PaginationContext.All;

            return query.Skip(pageContext.PageSize * (pageContext.PageNumber - 1))
                        .Take(pageContext.PageSize);
        }

        public async Task Create(T item)
        {
            await Create(new[] { item });
        }

        public async Task Update(T item)
        {
            await Update(new[] { item });
        }

        public async Task Remove(T item)
        {
            await Remove(new[] { item });
        }

        public async Task Create(IEnumerable<T> array)
        {
            await _Create(array);
            await Context.SaveChangesAsync();
        }

        public async Task Update(IEnumerable<T> array)
        {
            _Update(array);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(IEnumerable<T> array)
        {
            _Remove(array);
            await Context.SaveChangesAsync();
        }
        /// <summary>
        /// Protects creating process by making it private,
        /// unique to specific Repository implementation way
        /// </summary>
        /// <param name="array">Elements array</param>
        /// <returns>Awaitable Task which represent create process</returns>
        private async Task _Create(IEnumerable<T> array)
        {
            await Set.AddRangeAsync(array);
        }
        /// <summary>
        /// Protects updating process by making it private,
        /// unique to specific Repository implementation way
        /// </summary>
        /// <param name="array">Elements array</param>
        private void _Update(IEnumerable<T> array)
        {
            Set.UpdateRange(array);
        }
        /// <summary>
        /// Protects deleting process by making it private,
        /// unique to specific Repository implementation way
        /// </summary>
        /// <param name="array">Elements array</param>
        private void _Remove(IEnumerable<T> array)
        {
            Set.RemoveRange(array);
        }
    }
}
