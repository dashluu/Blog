using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public interface IBaseRepository<T>
    {
        bool Add(T entity);
        List<T> GetAll();
        bool SaveChanges();
    }
}
