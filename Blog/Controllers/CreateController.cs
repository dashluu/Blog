using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class CreateController : Controller
    {
        private IPostService postService;

        public CreateController(IPostService postService)
        {
            this.postService = postService;
        }

        // GET: Create
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(EditedPostDTO Post)
        {
            Post.CreatedDate = DateTime.Now;
            Post.UpdatedDate = DateTime.Now;
            Post.PostCategory = "Life";
            postService.AddEditedPostDTO(Post);
            return RedirectToAction("Index", "Home");
        }
    }
}