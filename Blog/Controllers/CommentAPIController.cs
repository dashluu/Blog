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

        public CommentAPIController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [Route("api/Comments")]
        public IHttpActionResult GetComments(int pageNumber, int pageSize)
        {
            object jsonObject;

            if (pageNumber <= 0 || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPaginationDTO(pageNumber, pageSize);

            if (commentPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = commentPaginationDTO
                };
            }

            return Json(jsonObject);
        }

        [Route("api/Comments/{commentId}/ChildComments")]
        public IHttpActionResult GetChildComments(string commentId, int pageNumber, int pageSize)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(commentId) || pageNumber <= 0 || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetChildCommentPaginationDTO(commentId, pageNumber, pageSize);

            if (commentPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = commentPaginationDTO
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Comments/Search")]
        public IHttpActionResult SearchComment([FromBody]APISearchModel searchModel, int pageNumber, int pageSize)
        {
            object jsonObject;
            string searchQuery = searchModel.Query;

            if (string.IsNullOrWhiteSpace(searchQuery) || pageNumber <= 0 || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.SearchCommentWithPaginationDTO(searchQuery, pageNumber, pageSize);

            if (commentPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = commentPaginationDTO
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public IHttpActionResult RemoveComment(string commentId)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(commentId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            bool removeSuccessfully = commentService.RemoveCommentDTO(commentId);

            if (!removeSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new { status = 200 };
            }

            return Json(jsonObject);
        }
    }
}
