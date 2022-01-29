using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using GenericRepository.Helpers.Utility;
using GenericRepository.Logic;

namespace GenericRepository.Application.Interfaces
{
    /// <summary>
    /// <para>Interface for Asynchronous Generic Repository Pattern</para>
    /// <para>Author: YANZAAN</para>
    /// <para>Source: https://github.com/YANZAAN/net5-async-generic-repository.git</para>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public interface IRepository<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Retrieves elements array specified by passed conditions and paginated
        /// </summary>
        /// <param name="condition">Output specification</param>
        /// <param name="paginationContext">Results pagination</param>
        /// <param name="includeProperties">Include resolvers to include all needed tables</param>
        /// <returns>Asynchronous accessor of T generic list</returns>
        IAsyncEnumerable<T> Get(Specification<T> condition, PaginationContext paginationContext, params Expression<Func<T, object>>[] includeProperties);
        /// <summary>
        /// Retrieves single element specified by passed conditions
        /// </summary>
        /// <param name="condition">Output specification</param>
        /// <param name="includeProperties">Include resolvers to include all needed tables</param>
        /// <returns>Asynchronous Task with T element result</returns>
        ValueTask<T> Find(Specification<T> condition, params Expression<Func<T, object>>[] includeProperties);
        /// <summary>
        /// Creates new element in database
        /// </summary>
        /// <param name="item">New element</param>
        /// <returns>Awaitable Task which represent create process</returns>
        Task Create(T item);
        /// <summary>
        /// Creates a list of element in database (Bulk create)
        /// </summary>
        /// <param name="array">Elements list</param>
        /// <returns>Awaitable Task which represent create process</returns>
        Task Create(IEnumerable<T> array);
        /// <summary>
        /// Update specified element
        /// </summary>
        /// <param name="item">Existing database element</param>
        /// <returns>Awaitable Task which represent update process</returns>
        Task Update(T item);
        /// <summary>
        /// Update a list of specified elements (Bulk update)
        /// </summary>
        /// <param name="array">Existing database element list</param>
        /// <returns>Awaitable Task which represent update process</returns>
        Task Update(IEnumerable<T> array);
        /// <summary>
        /// Removes specified element from database
        /// </summary>
        /// <param name="item">Existing database element</param>
        /// <returns>Awaitable Task which represent delete process</returns>
        Task Remove(T item);
        /// <summary>
        /// Removes specified element list from database
        /// </summary>
        /// <param name="array">Existing database element list</param>
        /// <returns>Awaitable Task which represent delete process</returns>
        Task Remove(IEnumerable<T> array);
    }
}
