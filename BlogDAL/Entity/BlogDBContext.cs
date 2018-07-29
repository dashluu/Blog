using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BlogDAL.Entity
{
    public class BlogDBContext : IdentityDbContext<UserEntity>
    {
        public DbSet<PostEntity> PostEntities { get; set; }
        public DbSet<CommentEntity> CommentEntities { get; set; }
        public DbSet<CategoryEntity> CategoryEntities { get; set; }

        public BlogDBContext() : base("BlogDB")
        {
        }

        public static BlogDBContext Create()
        {
            return new BlogDBContext();
        }

        static BlogDBContext()
        {
            Database.SetInitializer(new BlogDBInit());
        }
    }

    public class BlogDBInit : DropCreateDatabaseIfModelChanges<BlogDBContext>
    {
        protected override void Seed(BlogDBContext context)
        {
            Init(context);
            base.Seed(context);
        }

        public void Init(BlogDBContext context)
        {
            UserStore<UserEntity> userStore = new UserStore<UserEntity>(context);
            UserManager<UserEntity> userManager = new UserManager<UserEntity>(userStore);

            RoleStore<RoleEntity> roleStore = new RoleStore<RoleEntity>(context);
            RoleManager<RoleEntity> roleManager = new RoleManager<RoleEntity>(roleStore);

            string adminRoleName = "admin";

            RoleEntity roleEntity = new RoleEntity(adminRoleName);
            roleManager.Create(roleEntity);

            UserEntity userEntity = new UserEntity()
            {
                UserName = "dat",
                Email = "dashluu9121997@gmail.com"
            };

            userManager.Create(userEntity, password: "aAaaa1");
            userEntity = userManager.FindByName(userEntity.UserName);
            userManager.AddToRole(userEntity.Id, adminRoleName);
        }
    }
}
