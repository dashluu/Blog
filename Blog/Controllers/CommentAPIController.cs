using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Blog.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class CommentAPIController : ApiController
    {
        private ICommentService commentService;
        private APIIModelDataMapper dataMapper;

        public CommentAPIController(ICommentService commentService, APIIModelDataMapper dataMapper)
        {
            this.commentService = commentService;
            this.dataMapper = dataMapper;
        }

        [Route("api/Comments")]
        public IHttpActionResult GetComments(int pageSize, int pageNumber = 1, string postId = null, string searchQuery = null)
        {
            object jsonObject;

            if (pageNumber <= 0 || pageSize < 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPagination(pageNumber, pageSize, postId: postId, searchQuery: searchQuery);

            if (commentPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                PaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

                jsonObject = new
                {
                    status = 200,
                    data = commentPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [Route("api/Comments/{commentId}/ChildComments")]
        public IHttpActionResult GetChildComments(string commentId, int pageSize, int pageNumber = 1)
        {
            object jsonObject;

            if (pageNumber <= 0 || pageSize < 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPagination(pageNumber, pageSize, commentId: commentId);

            if (commentPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                PaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

                jsonObject = new
                {
                    status = 200,
                    data = commentPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [HttpDelete]
        [Route("api/Comments/{commentId}")]
        public IHttpActionResult RemoveComment(string commentId, int pageNumber = 1, int pageSize = 0, string postId = null)
        {
            object jsonObject;

            if (pageNumber <= 0 || pageSize < 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            bool removeSuccessfully = commentService.Remove(commentId);

            if (!removeSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPagination(pageNumber, pageSize, postId: postId);
                PaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

                jsonObject = new
                {
                    status = 200,
                    data = commentPaginationModel
                };
            }

            return Json(jsonObject);
        }
    }
}
