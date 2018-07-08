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

        public string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public virtual bool Add(T entity)
        {
            try
            {
                Context.Set<T>().Add(entity);
                Context.SaveChanges();
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
                int count = queryable.Count();
                int pages = (int)(Math.Ceiling((double)count / pageSize));
                int pageNumber = (skip / pageSize) + 1;

                if (isDesc)
                {
                    queryable = queryable.OrderByDescending(orderByExpression);
                }
                else
                {
                    queryable = queryable.OrderBy(orderByExpression);
                }

                if (skip >= count)
                {
                    skip = (pages - 1) * pageSize;
                }

                if (skip < 0)
                {
                    skip = 0;
                }

                if (pageNumber > pages)
                {
                    pageNumber = pages;
                }

                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                entities = queryable.Skip(skip).Take(pageSize).ToList();

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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }

        public bool Remove(T entity)
        {
            try
            {
                DbSet<T> set = Context.Set<T>();
                set.Attach(entity);
                set.Remove(entity);
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
                DbSet<T> set = Context.Set<T>();
                set.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
                Context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
