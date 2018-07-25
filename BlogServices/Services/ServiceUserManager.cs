using BlogDAL.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogServices.Services
{
    public class ServiceUserManager : UserManager<UserEntity>
    {
        private UserStore<UserEntity> userStore;

        public ServiceUserManager(UserStore<UserEntity> userStore) : base(userStore)
        {
            this.userStore = userStore;
        }

        public static ServiceUserManager Create()
        {
            BlogDBContext blogDBContext = new BlogDBContext();
            UserStore<UserEntity> userStore = new UserStore<UserEntity>(blogDBContext);
            return new ServiceUserManager(userStore);
        }

        public IQueryable<UserEntity> GetUserQueryable()
        {
            return userStore.Users;
        }
    }
}
