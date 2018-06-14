using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BlogDAL.Entity
{
    public class BlogDBContext : DbContext
    {
        public DbSet<PostEntity> PostEntities { get; set; }
        public DbSet<CommentEntity> CommentEntities { get; set; }
        public DbSet<CategoryEntity> CategoryEntities { get; set; }
    }
}
