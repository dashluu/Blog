using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class PostController : Controller
    {
        private IPostService postService;
        private ICommentService commentService;
        private IModelDataMapper dataMapper;

        public PostController(IPostService postService, ICommentService commentService, IModelDataMapper dataMapper)
        {
            this.postService = postService;
            this.commentService = commentService;
            this.dataMapper = dataMapper;
        }

        // GET: Default
        public ActionResult Index(string postId)
        {
            PostDTO postDTO = postService.GetPostDTO(postId);
            PostModel postModel = dataMapper.MapPostDTOToModel(postDTO);

            return View(postModel);
        }

        private bool StrNullOrEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        [HttpPost]
        public ActionResult MasterComment(string comment, string postId)
        {
            object jsonObject;

            if (StrNullOrEmpty(comment) || StrNullOrEmpty(postId))
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                string username = "whatever";
                string commentId = commentService.AddCommentDTO(postId, comment, username);

                if (commentId == null)
                {
                    jsonObject = new { status = 500 };
                }

                jsonObject = new
                {
                    data = comment,
                    status = 200,
                    username,
                    commentId
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public ActionResult ChildComment(string comment, string commentId, string postId)
        {
            object jsonObject;

            if (StrNullOrEmpty(comment) || StrNullOrEmpty(postId) || StrNullOrEmpty(commentId))
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                string username = "whatever";
                bool addSuccessfully = commentService.AddChildCommentDTO(postId, commentId, comment, username);

                if (!addSuccessfully)
                {
                    jsonObject = new { status = 500 };
                }

                jsonObject = new
                {
                    data = comment,
                    status = 200,
                    username
                };
            }

            return Json(jsonObject);
        }

        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(EditedPostModel Post)
        {
            Post.PostCategory = "Life";
            EditedPostDTO editedPostDTO = dataMapper.MapEditedPostModelToDTO(Post);
            postService.AddEditedPostDTO(editedPostDTO);

            return RedirectToAction("Index", "Home");
        }
    }
}