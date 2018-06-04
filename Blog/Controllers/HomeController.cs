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
        private IBlogService blogService;

        public HomeController()
        {
            blogService = new BlogService();
        }

        public ActionResult Index()
        {
            List<PostCardDTO> postCardDTOs = blogService.GetPostCardDTOs();
            return View(postCardDTOs);
        }

        [HttpPost]
        public ActionResult Index(string postId)
        {
            TempData["Post"] = blogService.GetPost(postId);
            return RedirectToAction("Index", "ViewPost");
        }

        private List<PostCardDTO> GetPostCardDTOsWithCategory(string category)
        {
            List<PostCardDTO> postCardList = blogService.GetPostCardDTOsWithCategory(category);
            return postCardList;
        }

        public ActionResult Life()
        {
            List<PostCardDTO> postCardList = GetPostCardDTOsWithCategory("Life");
            return View("Index", postCardList);
        }

        [HttpPost]
        public ActionResult Life(string postId)
        {
            TempData["Post"] = blogService.GetPost(postId);
            return RedirectToAction("Index", "ViewPost");
        }

        public ActionResult Travel()
        {
            List<PostCardDTO> postCardList = GetPostCardDTOsWithCategory("Travel");
            return View("Index", postCardList);
        }

        [HttpPost]
        public ActionResult Travel(string postId)
        {
            TempData["Post"] = blogService.GetPost(postId);
            return RedirectToAction("Index", "ViewPost");
        }

        public ActionResult Tech()
        {
            List<PostCardDTO> postCardList = GetPostCardDTOsWithCategory("Tech");
            return View("Index", postCardList);
        }

        [HttpPost]
        public ActionResult Tech(string postId)
        {
            TempData["Post"] = blogService.GetPost(postId);
            return RedirectToAction("Index", "ViewPost");
        }

        public ActionResult Health()
        {
            List<PostCardDTO> postCardList = GetPostCardDTOsWithCategory("Health");
            return View("Index", postCardList);
        }

        [HttpPost]
        public ActionResult Health(string postId)
        {
            TempData["Post"] = blogService.GetPost(postId);
            return RedirectToAction("Index", "ViewPost");
        }
    }
}