using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogServices.Services;
using BlogServices.DTO;
using Blog.Models;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Net;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IUserService userService;
        private IPostService postService;
        private ICategoryService categoryService;
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

        public HomeController(IUserService userService, IPostService postService, ICategoryService categoryService, IModelDataMapper dataMapper)
        {
            this.userService = userService;
            this.postService = postService;
            this.categoryService = categoryService;
            this.dataMapper = dataMapper;
        }

        public ActionResult Index()
        {
            UpdateAuthView();
            List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs = postService.GetPostCardPaginationList(pageSize: Settings.HOME_POST_PAGE_SIZE);
            List<PaginationModel<PostCardModel>> postCardPaginationModels = dataMapper.MapPostCardPaginationDTOsToModels(postCardPaginationDTOs);

            return View(postCardPaginationModels);
        }

        [Route("Categories/{category}")]
        public ActionResult Category(string category)
        {
            UpdateAuthView();
            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber: 1, pageSize: Settings.POST_PAGE_SIZE, category);
            PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

            return View(postCardPaginationModel);
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

        [HttpPost]
        [Route("Categories/{category}")]
        public ActionResult PostGrid(string category, int pageNumber = 1, string searchQuery = null)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(category) 
                || pageNumber <= 0)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber, Settings.HOME_POST_PAGE_SIZE, category, searchQuery);

            if (postCardPaginationDTO == null)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = postCardPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [ChildActionOnly]
        public ActionResult NavBar(string returnUrl, string userName, bool isAuthenticated)
        {
            List<CategoryDTO> categoryDTOs = categoryService.GetCategories();
            List<CategoryModel> categoryModels = dataMapper.MapCategoryDTOsToModels(categoryDTOs);

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.UserName = userName;
            ViewBag.IsAuthenticated = isAuthenticated;

            return PartialView("_NavBar", categoryModels);
        }

    }
}