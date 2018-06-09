using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogServices.Services;
using BlogServices.DTO;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IPostService postService;

        public HomeController(IPostService postService)
        {
            this.postService = postService;
        }

        public ActionResult Index()
        {
            List<PostCardDTO> postCardDTOs = postService.GetPostCardDTOs();
            return View(postCardDTOs);
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
            List<PostCardDTO> postCardList = postService.GetPostCardDTOsWithCategory(category);
            return View("Category", postCardList);
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