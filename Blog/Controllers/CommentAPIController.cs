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
        public IHttpActionResult GetComments(int pageSize, int pageNumber = 1, string postId = null)
        {
            object jsonObject;

            if (pageNumber <= 0 || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPaginationDTO(pageNumber, pageSize, postId);
            PaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

            if (commentPaginationDTO == null)
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

        [Route("api/Comments/{commentId}/ChildComments")]
        public IHttpActionResult GetChildComments(string commentId, int pageSize, int pageNumber = 1)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(commentId) 
                || pageNumber <= 0 
                || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetChildCommentPaginationDTO(commentId, pageNumber, pageSize);
            PaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

            if (commentPaginationDTO == null)
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
        [Route("api/Comments/Search")]
        public IHttpActionResult SearchComment([FromBody]APISearchModel searchModel, int pageSize, int pageNumber = 1, string postId = null)
        {
            object jsonObject;
            string searchQuery = searchModel.Query;

            if (pageNumber <= 0 || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.SearchCommentWithPaginationDTO(searchQuery, pageNumber, pageSize, postId);
            PaginationModel<APICommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

            if (commentPaginationDTO == null)
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

        [HttpDelete]
        [Route("api/Comments/{commentId}")]
        public IHttpActionResult RemoveComment(string commentId, int pageNumber = 1, int pageSize = 0, string postId = null)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(commentId)
                || pageNumber <= 0
                || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            bool removeSuccessfully = commentService.RemoveComment(commentId);

            if (!removeSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPaginationDTO(pageNumber, pageSize, postId);
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
