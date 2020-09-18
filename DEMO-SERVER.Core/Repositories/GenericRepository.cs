using DEMO.Core.DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DEMO_SERVER.Core.Repositories
{
    internal class GenericRepository<T> where T : BaseEntity
    {
        protected readonly DbContext db = null;
        //temporary solution to resolve child navigational property
        public List<string> ChildNavigationalProperties { get; set; } = new List<string>();

        public GenericRepository(DbContext context)
        {
            this.db = context;
        }

        public virtual IQueryable<T> GetCollections()
        {
            return ResolveNavigationProperties(db.Set<T>());
        }

        public virtual DbContext GetDatabase()
        {
            return db;
        }

        public virtual async Task<int> AddAsync(T model)
        {
            try
            {
                db.Set<T>().Add(model);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<int> AddRangeAsync(IEnumerable<T> models)
        {
            try
            {
                db.Set<T>().AddRange(models);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<int> UpdateAsync(T oldValue, T newValue)
        {
            try
            {
                db.Entry(oldValue).CurrentValues.SetValues(newValue);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Update status to deleted
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(T model)
        {
            try
            {
                model.RecordStatus = BaseEntity.RecordStatusType.Deleted;
                db.Set<T>().Attach(model);
                db.Entry<T>(model).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete list of records from the database.
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual async Task<int> ConcreteDeleteRangeAsync(IEnumerable<T> models)
        {
            try
            {
                db.Set<T>().RemoveRange(models);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete record from the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<int> ConcreteDeleteAsync(T model)
        {
            try
            {
                db.Set<T>().Remove(model);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual IEnumerable<T> GetList(Expression<Func<T, bool>> condition)
        {
            var collections = GetCollections();
            return collections.Where(condition);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(bool includeDeletedRecords = false)
        {
            var collections = GetCollections();
            if (includeDeletedRecords)
            {
                return await collections.ToListAsync();
            }
            else
            {
                return await collections.Where(x => x.RecordStatus != BaseEntity.RecordStatusType.Deleted).ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> condition)
        {
            var collections = GetCollections();
            return await collections.Where(condition).ToListAsync();
        }

        public virtual async Task<T> GetFirstAsync(Expression<Func<T, bool>> condition)
        {
            var collections = GetCollections();
            return await collections.FirstOrDefaultAsync(condition);
        }

        public IQueryable<T> ResolveNavigationProperties(IQueryable<T> query)
        {
            var navigationalProps = GetNavigationlProperties();
            return navigationalProps.Aggregate(query, (current, include) => current.Include(include));
        }

        private List<string> GetNavigationlProperties()
        {
            //only support navigational properties on main entity. child navigational properties should be add on ChildNavigationalProperties property on this class
            var entityType = typeof(T);
            var elementType = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<T>().EntitySet.ElementType;
            var baseNavigationalProperties = elementType.NavigationProperties.Select(property => entityType.GetProperty(property.Name).Name).ToList();

            //temporary solution to resolve child navigational property
            if (ChildNavigationalProperties.Count > 0)
            {
                baseNavigationalProperties.AddRange(ChildNavigationalProperties);
            }

            return baseNavigationalProperties;
        }
    }
}
