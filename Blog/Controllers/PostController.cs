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
            if (StrNullOrEmpty(postId))
            {
                //Do something with this exception.
            }

            PostDTOWithPaginatedComments postDTOWithPaginatedComments = postService.GetPostDTOWithPaginatedComments(postId, pageSize: pagination.CommentPageSize);
            PostModelWithPaginatedComments postModelWithPaginatedComments = dataMapper.MapPostDTOToModelWithPaginatedComments(postDTOWithPaginatedComments);

            if (postModelWithPaginatedComments == null || postModelWithPaginatedComments.Post == null)
            {
                //Do something with this exception.
            }

            return View(postModelWithPaginatedComments);
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

            if (StrNullOrEmpty(comment) || StrNullOrEmpty(postId) || StrNullOrEmpty(commentId))
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

            if (StrNullOrEmpty(postId) || skip < 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPaginationDTOWithPost(postId, skip, pageSize: pagination.CommentPageSize);
            List<CommentDTO> commentDTOs = commentPaginationDTO.DTOs;
            List<CommentModel> commentModels = dataMapper.MapCommentDTOsToModels(commentDTOs);
            bool hasNext = commentPaginationDTO.HasNext;

            if (commentModels == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = commentModels,
                    hasNext
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public ActionResult ChildComments(string commentId)
        {
            object jsonObject;

            if (StrNullOrEmpty(commentId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            List<CommentDTO> childCommentDTOs = commentService.GetChildCommentDTOs(commentId);
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
        public ActionResult CreatePost(EditedPostModel Post)
        {
            List<CategoryModel> categoryModels = GetCategoryModelsHelper();

            if (!ModelState.IsValid)
            {
                //Do something with this exception.
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

            if (StrNullOrEmpty(name) || StrNullOrEmpty(description))
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