using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Entity
{
    public class RoleEntity : IdentityRole
    {
        public RoleEntity() : base()
        {
        }

        public RoleEntity(string name) : base(name)
        {
        }
    }
}
