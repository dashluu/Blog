using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class PostController : Controller
    {
        private IPostService postService;
        private ICommentService commentService;
        private ICategoryService categoryService;
        private IModelDataMapper dataMapper;
        private Pagination pagination;

        public PostController(IPostService postService, ICommentService commentService, ICategoryService categoryService, IModelDataMapper dataMapper, Pagination pagination)
        {
            this.postService = postService;
            this.commentService = commentService;
            this.categoryService = categoryService;
            this.dataMapper = dataMapper;
            this.pagination = pagination;
        }

        // GET: Default
        public ActionResult Index(string postId)
        {
            if (string.IsNullOrWhiteSpace(postId))
            {
                //Do something with this exception.
            }

            PostDTOWithPaginatedComments postDTOWithPaginatedComments = postService.GetPostDTOWithPaginatedComments(postId, pageSize: pagination.CommentPageSize);
            PostModelWithPaginatedComments postModelWithPaginatedComments = dataMapper.MapPostDTOToModelWithPaginatedComments(postDTOWithPaginatedComments);

            return View(postModelWithPaginatedComments);
        }

        [HttpPost]
        public ActionResult MasterComment(string comment, string postId)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(comment) || string.IsNullOrWhiteSpace(postId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            string username = "whatever";
            string commentId = commentService.AddCommentDTO(postId, comment, username);

            if (commentId == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
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

            if (string.IsNullOrWhiteSpace(comment) || string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(commentId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            string username = "whatever";
            bool addSuccessfully = commentService.AddChildCommentDTO(postId, commentId, comment, username);

            if (!addSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    data = comment,
                    status = 200,
                    username
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public JsonResult PaginateComment(string postId, int skip)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(postId) || skip < 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPaginationDTOWithPost(postId, skip, pageSize: pagination.CommentPageSize);

            if (commentPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            List<CommentDTO> commentDTOs = commentPaginationDTO.DTOs;
            List<CommentModel> commentModels = dataMapper.MapCommentDTOsToModels(commentDTOs);
            bool hasNext = commentPaginationDTO.HasNext;

            jsonObject = new
            {
                status = 200,
                data = commentModels,
                hasNext
            };

            return Json(jsonObject);
        }

        [HttpPost]
        public ActionResult ShowChildComments(string commentId, int skip)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(commentId) || skip < 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            List<CommentDTO> childCommentDTOs = commentService.GetChildCommentDTOs(commentId, skip);
            List<CommentModel> childCommentModels = dataMapper.MapCommentDTOsToModels(childCommentDTOs);

            if (childCommentModels == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = childCommentModels
                };
            }

            return Json(jsonObject);
        }

        private List<CategoryModel> GetCategoryModelsHelper()
        {
            List<CategoryDTO> categoryDTOs = categoryService.GetCategoryDTOs();
            List<CategoryModel> categoryModels = dataMapper.MapCategoryDTOsToModels(categoryDTOs);

            return categoryModels;
        }

        public ActionResult CreatePost()
        {
            List<CategoryModel> categoryModels = GetCategoryModelsHelper();
            return View(categoryModels);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreatePost(EditedPostModel Post)
        {
            List<CategoryModel> categoryModels = GetCategoryModelsHelper();

            if (string.IsNullOrWhiteSpace(Post.Title))
            {
                ModelState.AddModelError("Title", "Title field is required.");
            }

            if (string.IsNullOrWhiteSpace(Post.ThumbnailImageSrc))
            {
                ModelState.AddModelError("Thumbnail", "Thumbnail image field is required.");
            }

            if (string.IsNullOrWhiteSpace(Post.ShortDescription))
            {
                ModelState.AddModelError("ShortDescription", "Short description field is required.");
            }

            if (string.IsNullOrWhiteSpace(Post.Content))
            {
                ModelState.AddModelError("Content", "Content field is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(categoryModels);
            }

            EditedPostDTO editedPostDTO = dataMapper.MapEditedPostModelToDTO(Post);
            postService.AddEditedPostDTO(editedPostDTO);

            return View(categoryModels);
        }

        [HttpPost]
        public ActionResult CreateCategory(string name, string description)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            EditedCategoryDTO editedCategoryDTO = new EditedCategoryDTO()
            {
                Name = name,
                Description = description
            };

            bool addSuccessfully = categoryService.AddEditedCategoryDTO(editedCategoryDTO);

            if (!addSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    name
                };
            }

            return Json(jsonObject);
        }
    }
}