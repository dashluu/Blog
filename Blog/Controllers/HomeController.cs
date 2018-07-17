using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogServices.Services;
using BlogServices.DTO;
using Blog.Models;
using Newtonsoft.Json;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IPostService postService;
        private ICategoryService categoryService;
        private IModelDataMapper dataMapper;

        public HomeController(IPostService postService, ICategoryService categoryService, IModelDataMapper dataMapper)
        {
            this.postService = postService;
            this.categoryService = categoryService;
            this.dataMapper = dataMapper;
        }

        public ActionResult Index()
        {
            List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs = postService.GetPostCardPaginationList(pageSize: Settings.HOME_POST_PAGE_SIZE);
            List<PaginationModel<PostCardModel>> postCardPaginationModels = dataMapper.MapPostCardPaginationDTOsToModels(postCardPaginationDTOs);

            return View(postCardPaginationModels);
        }

        [HttpPost]
        public ActionResult Index(string postId)
        {
            if (string.IsNullOrWhiteSpace(postId))
            {
                //Do something with this exception.
            }

            object package = new { postId };

            return RedirectToAction("Index", "Post", package);
        }

        public ActionResult Category(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                //Do something with this exception.
            }

            ViewBag.Category = category;

            return View();
        }

        [ChildActionOnly]
        public ActionResult CategoryPartial(string category, string searchQuery = null)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                //Do something with this exception.
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber: 1, pageSize: Settings.POST_PAGE_SIZE, category, searchQuery);
            PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

            return PartialView("_CategoryPartial", postCardPaginationModel);
        }

        [HttpPost]
        public ActionResult CategoryPartial(string category, int pageNumber, int pageSize, string searchQuery = null)
        {
            object jsonObject;

            if (pageNumber <= 0 || string.IsNullOrWhiteSpace(category))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber, pageSize, category, searchQuery);
            PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

            if (postCardPaginationModel == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = postCardPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public ActionResult ViewPost(string postId)
        {
            if (string.IsNullOrWhiteSpace(postId))
            {
                //Do something with this exception.
            }

            object package = new { postId };

            return RedirectToAction("Index", "Post", package);
        }

        [ChildActionOnly]
        public ActionResult NavBar()
        {
            List<CategoryDTO> categoryDTOs = categoryService.GetCategories();
            List<CategoryModel> categoryModels = dataMapper.MapCategoryDTOsToModels(categoryDTOs);

            return PartialView("_NavBar", categoryModels);
        }

    }
}