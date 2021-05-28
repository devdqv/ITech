using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace ITech.Models
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        public IUnitOfWork UnitOfWork { get; set; }
        private DbSet<T> _dbSet;
        private DbSet<T> DbSet
        {
            get
            {
                if (_dbSet == null)
                {
                    _dbSet = UnitOfWork.Context.Set<T>();
                }
                return _dbSet;
            }
        }

        public virtual IQueryable<T> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<T> All(string includes)
        {
            DbQuery<T> Query = UnitOfWork.Context.Set<T>();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (string includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(includeProperty.Trim());
                }
            }
            return Query;
        }

        public virtual IQueryable<T> All(List<string> lstInclude)
        {
            DbQuery<T> Query = UnitOfWork.Context.Set<T>();
            foreach (string itemInclude in lstInclude)
                Query = Query.Include(itemInclude.Trim());
            return Query;
        }

        public IQueryable<T> Find(Func<T, bool> expression)
        {
            return DbSet.Where(expression).AsQueryable();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Save()
        {
            UnitOfWork.Save();
        }

        public void AddRange(IEnumerable<T> list)
        {
            this.DbSet.AddRange(list);
        }

        public void RemoveRange(IEnumerable<T> list)
        {
            this.DbSet.RemoveRange(list);
        }

        public T FindByPK(params object[] keyValues)
        {
            return this.DbSet.Find(keyValues);
        }
    }
}