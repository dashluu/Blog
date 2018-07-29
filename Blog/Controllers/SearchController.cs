using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class SearchController : Controller
    {
        private IUserService userService;
        private IPostService postService;
        private IModelDataMapper dataMapper;

        private IUserService UserService
        {
            get
            {
                ServiceUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ServiceUserManager>();
                IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                userService.SetUserManager(userManager);
                userService.SetAuthManager(authManager);

                return userService;
            }
        }

        public SearchController(IUserService userService, IPostService postService, IModelDataMapper dataMapper)
        {
            this.userService = userService;
            this.postService = postService;
            this.dataMapper = dataMapper;
        }

        // GET: Search
        [Route("Search")]
        public ActionResult Index()
        {
            UpdateAuthView();
            return View();
        }

        [HttpPost]
        [Route("Search")]
        public ActionResult Index(string searchQuery)
        {
            List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs = postService.GetPostCardPaginationList(pageSize: Settings.HOME_POST_PAGE_SIZE, searchQuery);
            List<PaginationModel<PostCardModel>> postCardPaginationModels = dataMapper.MapPostCardPaginationDTOsToModels(postCardPaginationDTOs);

            return PartialView("~/Views/Home/_PostGridList.cshtml", postCardPaginationModels);
        }

        private void UpdateAuthView()
        {
            AuthDTO authDTO = UserService.GetAuth();

            if (authDTO.IsAuthenticated)
            {
                bool lockoutEnabled = UserService.LockoutEnabled(authDTO.UserName);

                if (lockoutEnabled)
                {
                    UserService.Logout();
                }

                ViewBag.IsAuthenticated = !lockoutEnabled;
            }
            else
            {
                ViewBag.IsAuthenticated = false;
            }

            ViewBag.ReturnUrl = Request.Url.AbsolutePath;
            ViewBag.UserName = authDTO.UserName;
        }
    }
}