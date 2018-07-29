﻿using BlogDAL.Entity;
using BlogServices.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<BlogDBContext>(BlogDBContext.Create);
            app.CreatePerOwinContext<ServiceUserManager>(ServiceUserManager.Create);
            app.CreatePerOwinContext<ServiceRoleManager>(ServiceRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }
    }
}