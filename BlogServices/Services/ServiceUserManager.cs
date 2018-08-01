using BlogDAL.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
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

        public static ServiceUserManager Create(IdentityFactoryOptions<ServiceUserManager> options, IOwinContext context)
        {
            BlogDBContext blogDBContext = context.Get<BlogDBContext>();
            UserStore<UserEntity> userStore = new UserStore<UserEntity>(blogDBContext);
            ServiceUserManager serviceUserManager = new ServiceUserManager(userStore);

            serviceUserManager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            serviceUserManager.UserValidator = new UserValidator<UserEntity>(serviceUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = false
            };

            return new ServiceUserManager(userStore);
        }

        public IQueryable<UserEntity> GetUserQueryable()
        {
            return userStore.Users;
        }
    }
}
