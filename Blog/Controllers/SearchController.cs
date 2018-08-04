using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                ServiceRoleManager roleManager = HttpContext.GetOwinContext().GetUserManager<ServiceRoleManager>();
                IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                userService.SetUserManager(userManager);
                userService.SetRoleManager(roleManager);
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
        public async Task<ActionResult> Index()
        {
            await UpdateAuthView();
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

        private async Task UpdateAuthView()
        {
            AuthDTO authDTO = UserService.GetAuth();
            bool isAuthenticated = authDTO.IsAuthenticated;
            bool lockoutEnabled = false;

            if (isAuthenticated)
            {
                lockoutEnabled = await UserService.LockoutEnabled(authDTO.UserName);

                if (lockoutEnabled)
                {
                    UserService.Logout();
                }
            }

            ViewBag.IsAuthenticated = isAuthenticated && !lockoutEnabled;
            ViewBag.ReturnUrl = Request.Url.AbsolutePath;
            ViewBag.UserName = authDTO.UserName;
        }
    }
}