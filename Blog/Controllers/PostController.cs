using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class PostController : Controller
    {
        private IUserService userService;
        private IPostService postService;
        private ICommentService commentService;
        private IModelDataMapper dataMapper;

        private IUserService UserService
        {
            get
            {
                ServiceUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ServiceUserManager>();
                IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                userService.SetUserManager(userManager);
                userService.SetAuthManager(authManager);

                return userService;
            }
        }

        public PostController(IUserService userService, IPostService postService, ICommentService commentService, IModelDataMapper dataMapper)
        {
            this.userService = userService;
            this.postService = postService;
            this.commentService = commentService;
            this.dataMapper = dataMapper;
        }

        // GET: Default
        [Route("Posts/{postId}")]
        public ActionResult Index(string postId)
        {
            UpdateAuthView();
            PostDTOWithPaginatedComments postDTOWithPaginatedComments = postService.GetPostWithPaginatedComments(postId, pageSize: Settings.COMMENT_PAGE_SIZE);
            PostModelWithPaginatedComments postModelWithPaginatedComments = dataMapper.MapPostDTOToModelWithPaginatedComments(postDTOWithPaginatedComments);

            return View(postModelWithPaginatedComments);
        }

        private void UpdateAuthView()
        {
            AuthDTO authDTO = UserService.GetAuth();

            if (authDTO.IsAuthenticated)
            {
                bool lockoutEnabled = UserService.LockoutEnabled(authDTO.UserName);

                if (lockoutEnabled)
                {
                    UserService.Logout();
                }

                ViewBag.IsAuthenticated = !lockoutEnabled;
            }
            else
            {
                ViewBag.IsAuthenticated = false;
            }

            ViewBag.ReturnUrl = Request.Url.AbsolutePath;
            ViewBag.UserName = authDTO.UserName;
        }

        [Authorize]
        [HttpPost]
        [Route("Comments")]
        public async Task<ActionResult> AddMasterComment(string comment, string postId)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(comment) 
                || string.IsNullOrWhiteSpace(postId))
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            AuthDTO authDTO = UserService.GetAuth();

            if (authDTO.IsAuthenticated)
            {
                bool lockoutEnabled = await UserService.LockoutEnabledAsync(authDTO.UserName);
                
                if (lockoutEnabled)
                {
                    UserService.Logout();

                    jsonObject = new
                    {
                        status = HttpStatusCode.Unauthorized
                    };

                    return Json(jsonObject);
                }
            }

            CommentDTO commentDTO = new CommentDTO()
            {
                Content = comment,
                PostId = postId,
                Username = HttpContext.User.Identity.Name
            };

            bool addSuccessfully = commentService.Add(commentDTO);

            if (!addSuccessfully)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                CommentModel commentModel = dataMapper.MapCommentDTOToModel(commentDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = commentModel
                };
            }

            return Json(jsonObject);
        }

        [Authorize]
        [HttpPost]
        [Route("Comments/{parentCommentId}/ChildComments/New")]
        public async Task<ActionResult> AddChildComment(string comment, string parentCommentId, string postId, bool loadChildComments)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(comment) 
                || string.IsNullOrWhiteSpace(postId))
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            AuthDTO authDTO = UserService.GetAuth();

            if (authDTO.IsAuthenticated)
            {
                bool lockoutEnabled = await UserService.LockoutEnabledAsync(authDTO.UserName);

                if (lockoutEnabled)
                {
                    UserService.Logout();

                    jsonObject = new
                    {
                        status = HttpStatusCode.Unauthorized
                    };

                    return Json(jsonObject);
                }
            }

            CommentDTO childCommentDTO = new CommentDTO()
            {
                Content = comment,
                PostId = postId,
                ParentCommentId = parentCommentId,
                Username = HttpContext.User.Identity.Name
            };

            bool addSuccessfully = commentService.Add(childCommentDTO);

            if (!addSuccessfully)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };

                return Json(jsonObject);
            }

            if (!loadChildComments)
            {
                CommentModel childCommentModel = dataMapper.MapCommentDTOToModel(childCommentDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = childCommentModel
                };
            }
            else
            {
                List<CommentDTO> childCommentDTOs = commentService.GetChildComments(parentCommentId);

                if (childCommentDTOs == null)
                {
                    jsonObject = new
                    {
                        status = HttpStatusCode.InternalServerError
                    };
                }
                else
                {
                    List<CommentModel> childCommentModels = dataMapper.MapCommentDTOsToModels(childCommentDTOs);

                    jsonObject = new
                    {
                        status = HttpStatusCode.OK,
                        data = childCommentModels
                    };
                }
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("Posts/{postId}/Comments/More")]
        public JsonResult ShowMoreComments(string postId, string createdDateString)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(createdDateString))
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            DateTime createdDate = dataMapper.ParseCommentTime(createdDateString);
            PaginationDTO<CommentDTO> commentPaginationDTO = commentService.GetCommentPaginationOfPostWithPreservedFetch(postId, createdDate, pageSize: Settings.COMMENT_PAGE_SIZE);

            if (commentPaginationDTO == null)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                PaginationModel<CommentModel> commentPaginationModel = dataMapper.MapCommentPaginationDTOToModel(commentPaginationDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
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
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                List<CommentModel> childCommentModels = dataMapper.MapCommentDTOsToModels(childCommentDTOs);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = childCommentModels
                };
            }

            return Json(jsonObject);
        }
    }
}