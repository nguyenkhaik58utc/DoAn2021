using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace iDAS.Core
{
    /// <summary>
    /// Interface BaseDA
    /// </summary>
    public interface IBaseDA<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Get Query
        /// </summary>
        IQueryable<TEntity> GetQuery(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "");

        /// <summary>
        /// Get List Entity
        /// </summary>
        List<TEntity> Get(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          string includeProperties = "");

        /// <summary>
        /// Get Entity By ID
        /// </summary>
        TEntity GetById(object id);

        /// <summary>
        /// Insert Entity
        /// </summary>
        void Insert(TEntity entity);

        /// <summary>
        /// Delete Entity By ID
        /// </summary>
        void Delete(object id);

        /// <summary>
        /// Delete Entity 
        /// </summary>
        /// <author>TungNT</author>
        /// <Date>15/04/2014</Date>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete All Entities By ID
        /// </summary>
        /// <author>TungNT</author>
        /// <Date>15/04/2014</Date>
        void DeleteRange(List<object> ids);

        /// <summary>
        /// Delete List Entity 
        /// </summary>
        /// <author>TungNT</author>
        /// <Date>15/04/2014</Date>
        void DeleteRange(List<TEntity> entities);

        /// <summary>
        /// Delete All Entity
        /// </summary>
        /// <author>TungNT</author>
        /// <Date>15/04/2014</Date>
        void DeleteAll();

        /// <summary>
        /// Update Entity
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Update Entity
        /// </summary>
        void UpdateRange(List<TEntity> entities);

        /// <summary>
        /// Save Changes To Database
        /// </summary>
        void Save();

    }
}
