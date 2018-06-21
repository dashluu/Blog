using System.Web.Mvc;
using Blog.Models;
using BlogDAL.Repository;
using BlogServices.DTO;
using BlogServices.Services;
using Microsoft.Practices.Unity;
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

            int homePostPageSize = 3;
            int postPageSize = 2;
            int commentPageSize = 1;
            int childCommentPageSize = 1;

            container.RegisterType<Pagination>(new InjectionConstructor(homePostPageSize, postPageSize, commentPageSize, childCommentPageSize));

            container.RegisterType<IModelDataMapper, ModelDataMapper>();
            container.RegisterType<IServiceDataMapper, ServiceDataMapper>();

            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<ICommentRepository, CommentRepository>();
            container.RegisterType<IPostRepository, PostRepository>();

            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IPostService, PostService>();
            container.RegisterType<ICommentService, CommentService>();

            return container;
        }
    }
}