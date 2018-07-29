using BlogDAL.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public class ServiceRoleManager : RoleManager<RoleEntity>
    {
        public RoleStore<RoleEntity> roleStore;

        public ServiceRoleManager(RoleStore<RoleEntity> roleStore) : base(roleStore)
        {
            this.roleStore = roleStore;
        }

        public static ServiceRoleManager Create(IdentityFactoryOptions<ServiceRoleManager> options, IOwinContext context)
        {
            BlogDBContext blogDBContext = context.Get<BlogDBContext>();
            RoleStore<RoleEntity> roleStore = new RoleStore<RoleEntity>(blogDBContext);
            ServiceRoleManager serviceRoleManager = new ServiceRoleManager(roleStore);

            return new ServiceRoleManager(roleStore);
        }
    }
}
