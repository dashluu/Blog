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
    public class PostAPIController : ApiController
    {
        private IPostService postService;
        private IModelDataMapper dataMapper;

        public PostAPIController(IPostService postService, IModelDataMapper dataMapper)
        {
            this.postService = postService;
            this.dataMapper = dataMapper;
        }

        [Route("api/Posts")]
        public IHttpActionResult GetPostCards(int pageSize, int pageNumber = 1, string category = null)
        {
            object jsonObject;

            if (pageNumber <= 0 || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPaginationDTO(category, pageNumber, pageSize);
            PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

            if (postCardPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = postCardPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [HttpDelete]
        [Route("api/Posts/{postId}")]
        public IHttpActionResult RemovePost(string postId, int pageNumber, int pageSize, string category = null)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(postId)
                || pageNumber <= 0
                || pageSize <= 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.RemovePostDTOWithReloadedPagination(category, postId, pageNumber, pageSize);
            PaginationModel<PostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

            if (postCardPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = postCardPaginationModel
                };
            }

            return Json(jsonObject);
        }
    }
}
