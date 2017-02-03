using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UltimateCoreWebAPI.Model.Abstractions;

namespace UltimateCoreWebAPI.Infrastructure.Abstractions
{
    public interface IDataRepository<T> where T: class, IPersistedData, new()
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includes);
        T Get(int id);
        T Get(Expression<Func<T, bool>> predicate);
        T GetIncluding(int id, params Expression<Func<T, object>>[] includes);
        T GetIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetQueryable();
        void Add(T data);
        void Edit(T data);
        void Delete(T data);
        void EnsureChanges();
    }
}
