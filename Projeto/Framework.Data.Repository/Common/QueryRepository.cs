using Framework.Data.Repository.Interfaces;
using Framework.Domain.Base;
using Framework.Domain.Base.Interfaces;
using Framework.Helper.Extension;
using Framework.Helper.Query;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Data.Repository.Common
{
    public class QueryRepository<TEntity, TId> : NhibernateRepositoryBase, IQueryRepository<TEntity, TId>
        where TEntity : IEntity<TId>
    {
        public IDataContext DataContext
        {
            get { return _dataContext; }
        }

        public QueryRepository(IDataContext dataContext)
            : base(dataContext)
        { }

        /// <summary>
        /// Validate if there is entity in database by expression
        /// </summary>
        /// <param name="expressao"></param>
        /// <returns>bool</returns>
        public virtual bool Exist(Expression<Func<TEntity, bool>> expressao)
        {
            return GetSession().Query<TEntity>().Any(expressao);
        }

        /// <summary>
        /// Validate if there is entity in database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public virtual bool Exist(TId id)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity));
            Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(Expression.Property(parameter, "Id"),
                                 Expression.Constant(id)), parameter);
            return GetSession().Query<TEntity>().Any(predicate);
        }

        /// <summary>
        /// Return count by expression
        /// </summary>
        /// <param name="expressao"></param>
        /// <returns>int</returns>
        public virtual int Count(Expression<Func<TEntity, bool>> expressao = null)
        {
            return expressao != null ?
                GetSession().Query<TEntity>().Count(expressao) :
                GetSession().Query<TEntity>().Count();
        }

        /// <summary>
        /// Return entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        public virtual TEntity Get(TId id)
        {
            return GetSession().Get<TEntity>(id);
        }

        /// <summary>
        /// Return complete entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        public virtual TEntity GetComplete(TId id)
        {
            return GetSession().Get<TEntity>(id);
        }

        public IList<TEntity> GetAllActive()
        {
            return GetAll(c => (c as EntityStatus<TId>).Active);
        }

        public IList<TEntity> GetAll(Expression<Func<TEntity, bool>> expression = null, Expression<Func<TEntity, bool>> ordemExpression = null)
        {
            IQueryable<TEntity> query = GetSession().Query<TEntity>();

            if (expression != null)
            {
                query.Where(expression);
            }

            if (ordemExpression != null)
            {
                query.OrderBy(ordemExpression);
            }

            return query.ToList();
        }

        public virtual IList<TEntity> GetAllPaged(int currentPage, int recordsPage)
        {
            return GetSession().Query<TEntity>()
                .Skip((currentPage - 1) * recordsPage)
                .Take(recordsPage)
                .ToList();
        }

        public virtual IList<TEntity> GetAllPaged(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> orderBy)
        {
            IQueryable<TEntity> query = GetSession().Query<TEntity>();

            if (filter != null)
            {
                query = filter.FilterQuery(query);
            }

            if (orderBy != null)
            {
                query = orderBy.OrderByQuery(query);
            }

            return query.Skip((currentPage - 1) * recordsPage)
                .Take(recordsPage)
                .ToList();
        }

        public virtual IList<TEntity> GetAllPagedComplete(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> orderBy)
        {
            IQueryable<TEntity> query = GetSession().Query(filter);

            return query
                .Order(orderBy)
                .Page(currentPage, recordsPage)
                .ToList();
        }

        #region Async

        public virtual async Task<bool> ExistAync(Expression<Func<TEntity, bool>> expressao)
        {
            return await GetSession().Query<TEntity>().AnyAsync(expressao);
        }

        public virtual async Task<bool> ExistAync(TId id)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity));
            Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(Expression.Property(parameter, "Id"),
                                 Expression.Constant(id)), parameter);
            return await GetSession().Query<TEntity>().AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expressao = null)
        {
            return await (expressao != null ?
                GetSession().Query<TEntity>().CountAsync(expressao) :
                GetSession().Query<TEntity>().CountAsync());
        }

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            return await GetSession().GetAsync<TEntity>(id);
        }

        public virtual async Task<TEntity> GetCompleteAsync(TId id)
        {
            return await GetSession().GetAsync<TEntity>(id);
        }

        public virtual async Task<IList<TEntity>> GetAllActiveAsync()
        {
            return await GetAllAsync(c => (c as EntityStatus<TId>).Active);
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null, Expression<Func<TEntity, bool>> ordemExpression = null)
        {
            IQueryable<TEntity> query = GetSession().Query<TEntity>();

            if (expression != null)
            {
                query.Where(expression);
            }

            if (ordemExpression != null)
            {
                query.OrderBy(ordemExpression);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllPagedAsync(int currentPage, int recordsPage)
        {
            return await GetSession().Query<TEntity>()
                .Skip((currentPage - 1) * recordsPage)
                .Take(recordsPage)
                .ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllPagedAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> orderBy)
        {
            IQueryable<TEntity> query = GetSession().Query<TEntity>();

            if (filter != null)
            {
                query = filter.FilterQuery(query);
            }

            if (orderBy != null)
            {
                query = orderBy.OrderByQuery(query);
            }

            return await query.Skip((currentPage - 1) * recordsPage)
                .Take(recordsPage)
                .ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllPagedCompleteAsync(int currentPage, int recordsPage, Filter<TEntity> filter, OrderCollection<TEntity> orderBy)
        {
            IQueryable<TEntity> query = GetSession().Query(filter);

            return await query
                .Order(orderBy)
                .Page(currentPage, recordsPage)
                .ToListAsync();
        }

        #endregion
    }
}