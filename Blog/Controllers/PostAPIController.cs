using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Blog.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [Authorize(Roles = "admin")]
    public class PostAPIController : ApiController
    {
        private IPostService postService;
        private APIIModelDataMapper dataMapper;

        public PostAPIController(IPostService postService, APIIModelDataMapper dataMapper)
        {
            this.postService = postService;
            this.dataMapper = dataMapper;
        }

        [Route("api/Posts")]
        public IHttpActionResult GetPostCards(int pageSize, int pageNumber = 1, string category = null, string searchQuery = null)
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

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber, pageSize, category: category, searchQuery: searchQuery);

            if (postCardPaginationDTO == null)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                APIPaginationModel<APIPostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = postCardPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [Route("api/Posts/{postId}")]
        public IHttpActionResult GetEditedPost(string postId)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(postId))
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            PostDTO postDTO = postService.GetPost(postId);

            if (postDTO == null)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                APIEditedPostModel editedPostModel = dataMapper.MapEditedPostDTOToModel(postDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = editedPostModel
                };
            }

            return Json(jsonObject);
        }

        [HttpDelete]
        [Route("api/Posts/{postId}")]
        public IHttpActionResult RemovePost(string postId, int pageNumber = 1, int pageSize = 0, string category = null)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(postId)
                || pageNumber <= 0 
                || pageSize < 0)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            bool removeSuccessfully = postService.Remove(postId);

            if (!removeSuccessfully)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber, pageSize, category: category);
                APIPaginationModel<APIPostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = postCardPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Posts")]
        public IHttpActionResult AddPost([FromBody]APIEditedPostModel editedPostModel)
        {
            object jsonObject;

            if (!ModelState.IsValid)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            PostDTO postDTO = dataMapper.MapEditedPostModelToDTO(editedPostModel);

            if (string.IsNullOrWhiteSpace(postDTO.ThumbnailImageSrc))
            {
                postDTO.ThumbnailImageSrc = APISettings.EMPTY_IMAGE;
            }

            bool addSuccessfully = postService.Add(postDTO);

            if (!addSuccessfully)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                jsonObject = new
                {
                    status = HttpStatusCode.OK
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Posts/{postId}")]
        public IHttpActionResult UpdatePost(string postId, [FromBody]APIEditedPostModel editedPostModel)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(postId) 
                || !ModelState.IsValid)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            PostDTO postDTO = dataMapper.MapEditedPostModelToDTO(editedPostModel);
            postDTO.PostId = postId;

            if (string.IsNullOrWhiteSpace(postDTO.ThumbnailImageSrc))
            {
                postDTO.ThumbnailImageSrc = APISettings.EMPTY_IMAGE;
            }

            bool updateSuccessfully = postService.Update(postDTO);

            if (!updateSuccessfully)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                jsonObject = new
                {
                    status = HttpStatusCode.OK
                };
            }

            return Json(jsonObject);
        }
    }
}
