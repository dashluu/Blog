﻿using Blog.Models;
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
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber, pageSize, category: category, searchQuery: searchQuery);

            if (postCardPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                APIPaginationModel<APIPostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

                jsonObject = new
                {
                    status = 200,
                    data = postCardPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [Route("api/Posts/{postId}")]
        public IHttpActionResult GetEditedPost(string postId)
        {
            object jsonObject;
            PostDTO postDTO = postService.GetPost(postId);

            if (postDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                APIEditedPostModel editedPostModel = dataMapper.MapEditedPostDTOToModel(postDTO);

                jsonObject = new
                {
                    status = 200,
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

            if (pageNumber <= 0 || pageSize < 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            bool removeSuccessfully = postService.Remove(postId);

            if (!removeSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                PaginationDTO<PostCardDTO> postCardPaginationDTO = postService.GetPostCardPagination(pageNumber, pageSize, category: category);
                APIPaginationModel<APIPostCardModel> postCardPaginationModel = dataMapper.MapPostCardPaginationDTOToModel(postCardPaginationDTO);

                jsonObject = new
                {
                    status = 200,
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
                jsonObject = new { status = 500 };
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
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new { status = 200 };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Posts/{postId}")]
        public IHttpActionResult UpdatePost(string postId, [FromBody]APIEditedPostModel editedPostModel)
        {
            object jsonObject;

            if (!ModelState.IsValid)
            {
                jsonObject = new { status = 500 };
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
