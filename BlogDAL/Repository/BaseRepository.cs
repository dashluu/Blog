using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

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

        public List<T> GetAll()
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

        public bool SaveChanges()
        {
            try
            {
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
