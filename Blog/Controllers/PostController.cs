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
        private IModelDataMapper dataMapper;
        private Pagination pagination;

        public PostController(IPostService postService, ICommentService commentService, IModelDataMapper dataMapper, Pagination pagination)
        {
            this.postService = postService;
            this.commentService = commentService;
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
        public ActionResult AddMasterComment(string comment, string postId)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(comment) || string.IsNullOrWhiteSpace(postId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            CommentDTO commentDTO = new CommentDTO()
            {
                Content = comment,
                PostId = postId
            };

            bool addSuccessfully = commentService.AddComment(commentDTO);

            if (!addSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                CommentModel commentModel = dataMapper.MapCommentDTOToModel(commentDTO);

                jsonObject = new
                {
                    status = 200,
                    data = commentModel
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public ActionResult AddChildComment(string comment, string commentId, string postId)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(comment) || string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(commentId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            CommentDTO childCommentDTO = new CommentDTO()
            {
                Content = comment,
                PostId = postId,
                ParentCommentId = commentId
            };

            bool addSuccessfully = commentService.AddComment(childCommentDTO);

            if (!addSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                CommentModel childCommentModel = dataMapper.MapCommentDTOToModel(childCommentDTO);

                jsonObject = new
                {
                    status = 200,
                    data = childCommentModel
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public JsonResult ShowMoreComments(string postId, string createdDateString)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(createdDateString))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            DateTime createdDate = dataMapper.ParseCommentTime(createdDateString);
            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPaginationDTOOfPostWithPreservedFetch(postId, createdDate, pageSize: pagination.CommentPageSize);
            PaginationModel<CommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

            if (commentPaginationModel == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = commentPaginationModel
                };
            }

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
    }
}