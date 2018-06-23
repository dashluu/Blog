using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Blog.Controllers
{
    public class PostAPIController : ApiController
    {
        private IPostService postService;

        public PostAPIController(IPostService postService)
        {
            this.postService = postService;
        }

        public IHttpActionResult Get()
        {
            List<PostCardDTO> postCardDTOs = postService.GetPostCardDTOs();
            return Json(postCardDTOs);
        }
    }
}
