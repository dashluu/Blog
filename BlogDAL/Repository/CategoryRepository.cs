using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;

namespace BlogDAL.Repository
{
    public class CategoryRepository : BaseRepository<CategoryEntity, BlogDBContext>, ICategoryRepository
    {
        public override bool Update(CategoryEntity categoryEntity)
        {
            try
            {
                Context.CategoryEntities.Attach(categoryEntity);
                DbEntityEntry<CategoryEntity> entry = Context.Entry(categoryEntity);
                entry.Property(x => x.Description).IsModified = true;
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
