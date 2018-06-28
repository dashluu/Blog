using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public class CategoryRepository : BaseRepository<CategoryEntity, BlogDBContext>, ICategoryRepository
    {
        public override bool Add(CategoryEntity entity)
        {
            entity.CategoryId = GenerateId();
            return base.Add(entity);
        }
    }
}
