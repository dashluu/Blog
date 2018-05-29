using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ViewPostController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            PostModel postModel = (PostModel)TempData["Post"];
            return View(postModel);
        }
    }
}