using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BlogDAL.Entity;

namespace BlogDAL.Repository
{
    public class BaseRepository<T, C> : IBaseRepository<T>
        where T : class
        where C : DbContext, new()
    {
        public C Context { get; set; }

        public BaseRepository()
        {
            Context = new C();
        }

        public virtual bool Add(T entity)
        {
            try
            {
                Context.Set<T>().Add(entity);
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
        }

        public List<T> GetEntities()
        {
            try
            {
                List<T> blogEntityList = Context.Set<T>().ToList();
                return blogEntityList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<T> GetPaginationEntity<TKey>
            (IQueryable<T> queryable, bool isDesc,
            Expression<Func<T, TKey>> orderByExpression,
            int skip, int pageSize)
        {
            try
            {
                List<T> entities = null;

                if (isDesc)
                {
                    entities = queryable.OrderByDescending(orderByExpression)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }
                else
                {
                    entities = queryable.OrderBy(orderByExpression)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }

                if (entities.Count == 0)
                {
                    return null;
                }

                int count = queryable.Count();

                PaginationEntity<T> paginationEntity = new PaginationEntity<T>()
                {
                    Entities = entities,
                    Pages = (int)(Math.Ceiling((double)count / pageSize)),
                    HasNext = (skip + pageSize) < count,
                    HasPrevious = skip > 0
                };

                return paginationEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
