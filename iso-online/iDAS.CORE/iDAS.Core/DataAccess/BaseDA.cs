using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace iDAS.Core
{
    /// <summary>
    /// BaseDA
    /// </summary>
    public class BaseDA<TEntity> : IDisposable, IBaseDA<TEntity>
        where TEntity : class
    {
        protected DbContext Context
        {
            get
            {
                return _Context;
            }
            set
            {
                _Context = value;
                if (_Context != null)
                    _DbSet = _Context.Set<TEntity>();
            }
        }

        private DbContext _Context;

        private DbSet<TEntity> _DbSet;

        private bool disposed = false;

        /// <summary>
        /// Contructor for BaseRepository without Context
        /// </summary>
        public BaseDA()
        {
        }

        /// <summary>
        /// Contructor for BaseRepository with Context
        /// </summary>
        public BaseDA(DbContext context)
        {
            this.Context = context;
            this._DbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Get Query for Linq Expressions
        /// </summary>
        public IQueryable<TEntity> GetQuery(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _DbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        /// <summary>
        /// Get List Entity
        /// </summary>
        public virtual List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = GetQuery(filter, orderBy, includeProperties);
            return query.ToList();
        }

        /// <summary>
        /// Get Entity By ID
        /// </summary>
        public virtual TEntity GetById(object id)
        {
            return _DbSet.Find(id);
        }

        /// <summary>
        /// Insert Entity
        /// </summary>
        public virtual void Insert(TEntity entity)
        {
            _DbSet.Add(entity);
        }

        /// <summary>
        /// Insert List Entity
        /// </summary>
        public virtual void InsertRange(List<TEntity> entities)
        {
            _DbSet.AddRange(entities);
        }

        /// <summary>
        /// Delete Entity By ID
        /// </summary>
        public virtual void Delete(object id)
        {
            TEntity entity = _DbSet.Find(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Delete Entity 
        /// </summary>
        public virtual void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                _DbSet.Attach(entity);
            }
            _DbSet.Remove(entity);
            Context.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// Delete All Entities By ID
        /// </summary>
        public virtual void DeleteRange(List<object> ids)
        {
            foreach (var id in ids)
            {
                Delete(id);
            }
        }

        /// <summary>
        /// Delete List Entity 
        /// </summary>
        public virtual void DeleteRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Delete All Entity
        /// </summary>
        public virtual void DeleteAll()
        {
            List<TEntity> entities = _DbSet.ToList<TEntity>();
            DeleteRange(entities);
        }

        /// <summary>
        /// Update Entity
        /// </summary>
        public virtual void Update(TEntity entity)
        {
            _DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Update List Entity
        /// </summary>
        public virtual void UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        /// <summary>
        /// Save Changes To Database
        /// </summary>
        public void Save()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// Refresh Entity
        /// </summary>
        public void Refresh()
        {
            var context = ((IObjectContextAdapter)this.Context).ObjectContext;
            context.Refresh(RefreshMode.StoreWins, _DbSet);
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
