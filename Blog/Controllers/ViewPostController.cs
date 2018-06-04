using BlogServices.DTO;
using BlogServices.Services;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ViewPostController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            if (TempData["Post"] == null)
            {
                return new EmptyResult();
            }
            PostDTO postModel = (PostDTO)TempData["Post"];
            return View(postModel);
        }

        [HttpPost]
        public ActionResult MasterComment(string comment, string postId)
        {
            object jsonObject = new
            {
                data = comment,
                status = 200,
                username = "Whatever",
                commentId = "xyz"
            };
            return Json(jsonObject);
        }

        [HttpPost]
        public ActionResult ChildComment(string comment, string commentId)
        {
            object jsonObject = new
            {
                data = comment,
                status = 200,
                username = "Whatever"
            };
            return Json(jsonObject);
        }
    }
}