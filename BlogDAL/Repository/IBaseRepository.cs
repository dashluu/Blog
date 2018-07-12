using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BlogDAL.Repository
{
    public interface IBaseRepository<T>
    {
        bool Add(T entity);
        bool Remove(T entity);
        bool Update(T entity);
        List<T> GetEntities();
        PaginationEntity<T> GetPaginationEntity<TKey>
            (IQueryable<T> queryable, bool isDesc,
            Expression<Func<T, TKey>> orderByExpression,
            int pageNumber, int pageSize);
    }
}
