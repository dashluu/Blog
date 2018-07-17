using Blog.Models;
using BlogDAL.Repository;
using BlogServices.DTO;
using BlogServices.Services;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace Blog
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IImageService, ImageService>();
            container.RegisterType<IHashService, HashService>();
            container.RegisterType<APIIModelDataMapper, APIModelDataMapper>();
            container.RegisterType<IServiceDataMapper, ServiceDataMapper>();

            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<ICommentRepository, CommentRepository>();
            container.RegisterType<IPostRepository, PostRepository>();

            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IPostService, PostService>();
            container.RegisterType<ICommentService, CommentService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}