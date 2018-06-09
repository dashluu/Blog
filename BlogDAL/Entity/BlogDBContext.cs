using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BlogDAL.Entity
{
    public class BlogDBContext : DbContext
    {
        public DbSet<PostEntity> PostEntities { get; set; }
    }
}
