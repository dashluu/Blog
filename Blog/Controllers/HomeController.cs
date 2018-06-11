using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogServices.Services;
using BlogServices.DTO;
using Blog.Models;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IPostService postService;
        private IModelDataMapper dataMapper;

        public HomeController(IPostService postService, IModelDataMapper dataMapper)
        {
            this.postService = postService;
            this.dataMapper = dataMapper;
        }

        public ActionResult Index()
        {
            List<PostCardDTO> postCardDTOs = postService.GetPostCardDTOs();
            List<PostCardModel> postCardModels = new List<PostCardModel>();

            foreach (PostCardDTO postCardDTO in postCardDTOs)
            {
                PostCardModel postCardModel = dataMapper.MapPostCardDTOToModel(postCardDTO);
                postCardModels.Add(postCardModel);
            }

            return View(postCardModels);
        }

        private bool StrNullOrEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        [HttpPost]
        public ActionResult Index(string postId)
        {
            if (StrNullOrEmpty(postId))
            {
                return RedirectToAction("Index");
            }

            object package = new { postId };

            return RedirectToAction("Index", "Post", package);
        }

        public ActionResult Category(string category)
        {
            List<PostCardDTO> postCardDTOs = postService.GetPostCardDTOsWithCategory(category);
            List<PostCardModel> postCardModels = new List<PostCardModel>();

            foreach (PostCardDTO postCardDTO in postCardDTOs)
            {
                PostCardModel postCardModel = dataMapper.MapPostCardDTOToModel(postCardDTO);
                postCardModels.Add(postCardModel);
            }

            return View("Category", postCardModels);
        }

        [HttpPost]
        public ActionResult CategoryPost(string postId)
        {
            if (StrNullOrEmpty(postId))
            {
                return RedirectToAction("Index");
            }

            object package = new { postId };

            return RedirectToAction("Index", "Post", package);
        }

    }
}