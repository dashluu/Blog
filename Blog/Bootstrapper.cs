using System.Web.Mvc;
using Blog.Models;
using BlogDAL.Repository;
using BlogServices.DTO;
using BlogServices.Services;
using Microsoft.Practices.Unity;
using Unity.Injection;
using Unity.Mvc3;

namespace Blog
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IHashService, HashService>();
            container.RegisterType<IModelDataMapper, ModelDataMapper>();
            container.RegisterType<IServiceDataMapper, ServiceDataMapper>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<ICommentRepository, CommentRepository>();
            container.RegisterType<IPostRepository, PostRepository>();

            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IPostService, PostService>();
            container.RegisterType<ICommentService, CommentService>();

            return container;
        }
    }
}