using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using NET.Repository.Full.Logic;
using NET.Repository.Full.Utility;

namespace NET.Repository.Full
{
    /// <summary>
    ///     <para>Full-variant repository interface</para>
    ///     <para>Source: https://github.com/YANZAAN/NET.Repository.git</para>
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IFullRepository<T> : IDisposable
        where T : class
    {   
        /// <summary>
        ///     Get entities by specified parameters
        /// </summary>
        /// <param name="condition">Specification class to filter entities</param>
        /// <param name="includeProperties">Resolvers array to include additional tables</param>
        /// <param name="pageContext">Context to generate entities page</param>
        /// <param name="orderBy">Order expression</param>
        /// <returns>Entity enumeration of T</returns>
        IEnumerable<T> Get(Specification<T> condition,
            PaginationContext pageContext,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] includeProperties);
        /// <summary>
        ///     Get single entity by specified parameters
        /// </summary>
        /// <param name="condition">Specification class to select entity</param>
        /// <param name="includeProperties">Resolvers array to include additional tables</param>
        /// <returns>Entity of T if it's present; default otherwise</returns>
        T Find(Specification<T> condition,
            params Expression<Func<T, object>>[] includeProperties);
        /// <summary>
        ///     Insert new entity
        /// </summary>
        /// <param name="entity">Entity of T</param>
        void Insert(T entity);
        /// <summary>
        ///     Shortcut to insert entity list
        /// </summary>
        /// <param name="entitiesToInsert">Entity enumeration of T</param>
        void Insert(IEnumerable<T> entitiesToInsert);
        /// <summary>
        ///     Update specified entity
        /// </summary>
        /// <param name="entityToUpdate">Entity of T</param>
        void Update(T entityToUpdate);
        /// <summary>
        ///     Update specified entity list
        /// </summary>
        /// <param name="entitiesToUpdate">Entity enumeration of T</param>
        void Update(IEnumerable<T> entitiesToUpdate);
        /// <summary>
        ///     Remove specified entity from database
        /// </summary>
        /// <param name="id">Entity id</param>
        void Delete(object id);
        /// <summary>
        ///     Remove specified entity from database
        /// </summary>
        /// <param name="entityToDelete">Entity of T</param>
        void Delete(T entityToDelete);
        /// <summary>
        ///     Removes specified entity list from database
        /// </summary>
        /// <param name="entitiesToDelete">Entity enumeration of T</param>
        void Delete(IEnumerable<T> entitiesToDelete);
    }
}