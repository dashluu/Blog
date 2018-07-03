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
        private Pagination pagination;

        public HomeController(IPostService postService, ICategoryService categoryService, IModelDataMapper dataMapper, Pagination pagination)
        {
            this.postService = postService;
            this.categoryService = categoryService;
            this.dataMapper = dataMapper;
            this.pagination = pagination;
        }

        public ActionResult Index()
        {
            List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs = postService.GetPostCardPaginationDTOs(pageSize: pagination.HomePostPageSize);
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
        public ActionResult CategoryPartial(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                //Do something with this exception.
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPaginationDTOWithCategory(category, pageNumber: 1, pageSize: pagination.PostPageSize);
            PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

            return PartialView("_CategoryPartial", postCardPaginationModel);
        }

        [HttpPost]
        public ActionResult CategoryPartial(string category, int pageNumber, bool nextPage)
        {
            object jsonObject;

            if (pageNumber <= 0 || string.IsNullOrWhiteSpace(category))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            if (nextPage)
            {
                pageNumber++;
            }
            else
            {
                pageNumber--;
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPaginationDTOWithCategory(category, pageNumber, pageSize: pagination.PostPageSize);

            if (postCardPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            List<PostCardDTO> postCardDTOs = postCardPaginationDTO.DTOs;
            List<PostCardModel> postCardModels = dataMapper.MapPostCardDTOsToModels(postCardDTOs);
            bool hasNext = postCardPaginationDTO.HasNext;
            bool hasPrevious = postCardPaginationDTO.HasPrevious;
            int pages = postCardPaginationDTO.Pages;

            jsonObject = new
            {
                status = 200,
                data = postCardModels,
                hasNext,
                hasPrevious,
                pages
            };

            return Json(jsonObject);
        }

        [HttpPost]
        public ActionResult CategoryPost(string postId)
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
            List<CategoryDTO> categoryDTOs = categoryService.GetCategoryDTOs();
            List<CategoryModel> categoryModels = dataMapper.MapCategoryDTOsToModels(categoryDTOs);

            return PartialView("_NavBar", categoryModels);
        }

    }
}