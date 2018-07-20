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

        [Route("Categories/{category}")]
        public ActionResult Category(string category)
        {
            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber: 1, pageSize: Settings.POST_PAGE_SIZE, category);
            PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

            return View(postCardPaginationModel);
        }

        [HttpPost]
        [Route("Categories/{category}")]
        public ActionResult PostGrid(string category, int pageNumber = 1, string searchQuery = null)
        {
            object jsonObject;

            if (pageNumber <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber, Settings.HOME_POST_PAGE_SIZE, category, searchQuery);

            if (postCardPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

                jsonObject = new
                {
                    status = 200,
                    data = postCardPaginationModel
                };
            }

            return Json(jsonObject);
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