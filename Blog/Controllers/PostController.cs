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

        public PostController(IPostService postService, ICommentService commentService, IModelDataMapper dataMapper)
        {
            this.postService = postService;
            this.commentService = commentService;
            this.dataMapper = dataMapper;
        }

        // GET: Default
        [Route("Posts/{postId}")]
        public ActionResult Index(string postId)
        {
            PostDTOWithPaginatedComments postDTOWithPaginatedComments = postService.GetPostWithPaginatedComments(postId, pageSize: Settings.COMMENT_PAGE_SIZE);
            PostModelWithPaginatedComments postModelWithPaginatedComments = dataMapper.MapPostDTOToModelWithPaginatedComments(postDTOWithPaginatedComments);

            return View(postModelWithPaginatedComments);
        }

        [HttpPost]
        [Route("Comments")]
        public ActionResult AddMasterComment(string comment, string postId)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(comment) 
                || string.IsNullOrWhiteSpace(postId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            CommentDTO commentDTO = new CommentDTO()
            {
                Content = comment,
                PostId = postId
            };

            bool addSuccessfully = commentService.Add(commentDTO);

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
        [Route("Comments/{parentCommentId}/ChildComments/New")]
        public ActionResult AddChildComment(string comment, string parentCommentId, string postId, bool loadChildComments)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(comment) 
                || string.IsNullOrWhiteSpace(postId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            CommentDTO childCommentDTO = new CommentDTO()
            {
                Content = comment,
                PostId = postId,
                ParentCommentId = parentCommentId
            };

            bool addSuccessfully = commentService.Add(childCommentDTO);

            if (!addSuccessfully)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            if (!loadChildComments)
            {
                CommentModel childCommentModel = dataMapper.MapCommentDTOToModel(childCommentDTO);

                jsonObject = new
                {
                    status = 200,
                    data = childCommentModel
                };
            }
            else
            {
                List<CommentDTO> childCommentDTOs = commentService.GetChildComments(parentCommentId);

                if (childCommentDTOs == null)
                {
                    jsonObject = new { status = 500 };
                }
                else
                {
                    List<CommentModel> childCommentModels = dataMapper.MapCommentDTOsToModels(childCommentDTOs);

                    jsonObject = new
                    {
                        status = 200,
                        data = childCommentModels
                    };
                }
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("{postId}/Comments/More")]
        public JsonResult ShowMoreComments(string postId, string createdDateString)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(createdDateString))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            DateTime createdDate = dataMapper.ParseCommentTime(createdDateString);
            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPaginationOfPostWithPreservedFetch(postId, createdDate, pageSize: Settings.COMMENT_PAGE_SIZE);

            if (commentPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                PaginationModel<CommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

                jsonObject = new
                {
                    status = 200,
                    data = commentPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("Comments/{parentCommentId}/ChildComments")]
        public ActionResult ShowChildComments(string parentCommentId)
        {
            object jsonObject;

            List<CommentDTO> childCommentDTOs = commentService.GetChildComments(parentCommentId);

            if (childCommentDTOs == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                List<CommentModel> childCommentModels = dataMapper.MapCommentDTOsToModels(childCommentDTOs);

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