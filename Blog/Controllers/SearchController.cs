using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class SearchController : Controller
    {
        private IPostService postService;
        private IModelDataMapper dataMapper;

        public SearchController(IPostService postService, IModelDataMapper dataMapper)
        {
            this.postService = postService;
            this.dataMapper = dataMapper;
        }

        // GET: Search
        [Route("Search")]
        public ActionResult Index()
        {
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
    }
}