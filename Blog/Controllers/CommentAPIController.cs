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
    [Authorize(Roles = "admin")]
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
        public IHttpActionResult GetComments(int pageSize, int pageNumber = 1, string postId = null, string userName = null, string searchQuery = null)
        {
            object jsonObject;

            if (pageNumber <= 0 || pageSize < 0)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPagination(pageNumber, pageSize, postId: postId, userName: userName, searchQuery: searchQuery);

            if (commentPaginationDTO == null)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                APIPaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
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
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPagination(pageNumber, pageSize, commentId: commentId);

            if (commentPaginationDTO == null)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                APIPaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = commentPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [HttpDelete]
        [Route("api/Comments/{commentId}")]
        public IHttpActionResult RemoveComment(string commentId, int pageNumber = 1, int pageSize = 0, string postId = null, string userName = null)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(commentId)
                || pageNumber <= 0 
                || pageSize < 0)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            bool removeSuccessfully = commentService.Remove(commentId);

            if (!removeSuccessfully)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPagination(pageNumber, pageSize, postId: postId, userName: userName);
                APIPaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = commentPaginationModel
                };
            }

            return Json(jsonObject);
        }
    }
}
