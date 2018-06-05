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
            TempData["Post"] = blogService.GetPostDTO(postId);
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
            if (StrNullOrEmpty(postId))
            {
                return RedirectToAction("Life");
            }
            TempData["Post"] = blogService.GetPostDTO(postId);
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
            if (StrNullOrEmpty(postId))
            {
                return RedirectToAction("Travel");
            }
            TempData["Post"] = blogService.GetPostDTO(postId);
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
            if (StrNullOrEmpty(postId))
            {
                return RedirectToAction("Tech");
            }
            TempData["Post"] = blogService.GetPostDTO(postId);
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
            if (StrNullOrEmpty(postId))
            {
                return RedirectToAction("Health");
            }
            TempData["Post"] = blogService.GetPostDTO(postId);
            return RedirectToAction("Index", "ViewPost");
        }
    }
}