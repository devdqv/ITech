

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ITech.Models
{
    public interface IRepository<T>
    {
        IUnitOfWork UnitOfWork { get; set; }
        IQueryable<T> All();
        IQueryable<T> All(string includes);
        IQueryable<T> All(List<string> lstInclude);
        IQueryable<T> Find(Func<T, bool> expression);
        void Add(T entity);
        void Delete(T entity);
        void Save();
        void AddRange(IEnumerable<T> list);
        void RemoveRange(IEnumerable<T> list);
        T FindByPK(params object[] keyValues);
    }
}

