using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.DataSample;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private DataSampleContainer dataSampleContainer = new DataSampleContainer();

        public ActionResult Index()
        {
            return View(dataSampleContainer.PostCardList);
        }

        [HttpPost]
        public ActionResult Index(string postId)
        {
            List<PostModel> postList = dataSampleContainer.PostList;
            foreach (PostModel post in postList)
            {
                if (post.PostId.Equals(postId))
                {
                    TempData["Post"] = post;
                }
            }
            return RedirectToAction("Index", "ViewPost");
        }


    }
}