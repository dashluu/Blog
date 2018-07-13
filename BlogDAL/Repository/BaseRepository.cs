using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
                Context.Set<T>().Attach(entity);
                DbEntityEntry<T> entry = Context.Entry(entity);
                entry.State = EntityState.Added;
                Context.SaveChanges();
                entry.State = EntityState.Detached;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<T> GetEntities()
        {
            try
            {
                List<T> blogEntityList = Context.Set<T>().AsNoTracking().ToList();
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
            int pageNumber, int pageSize)
        {
            try
            {
                List<T> entities = null;
                int count = queryable.Count();
                int pages = (int)(Math.Ceiling((double)count / pageSize));

                if (isDesc)
                {
                    queryable = queryable.OrderByDescending(orderByExpression);
                }
                else
                {
                    queryable = queryable.OrderBy(orderByExpression);
                }

                if (pageNumber > pages)
                {
                    pageNumber = pages;
                }

                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                int skip = (pageNumber - 1) * pageSize;

                entities = queryable.AsNoTracking().Skip(skip).Take(pageSize).ToList();

                PaginationEntity<T> paginationEntity = new PaginationEntity<T>()
                {
                    Entities = entities,
                    Pages = pages,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
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

        public bool Remove(T entity)
        {
            try
            {
                Context.Set<T>().Attach(entity);
                DbEntityEntry<T> entry = Context.Entry(entity);
                entry.State = EntityState.Deleted;
                Context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool Update(T entity)
        {
            try
            {
                Context.Set<T>().Attach(entity);
                DbEntityEntry<T> entry = Context.Entry(entity);
                entry.State = EntityState.Modified;
                Context.SaveChanges();
                entry.State = EntityState.Detached;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
