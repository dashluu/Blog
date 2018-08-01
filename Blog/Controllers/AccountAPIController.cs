using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Blog.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class AccountAPIController : ApiController
    {
        private IUserService userService;
        private APIIModelDataMapper dataMapper;

        private IUserService UserService
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;
                ServiceUserManager userManager = httpContext.GetOwinContext().GetUserManager<ServiceUserManager>();
                ServiceRoleManager roleManager = httpContext.GetOwinContext().GetUserManager<ServiceRoleManager>();
                IAuthenticationManager authManager = httpContext.GetOwinContext().Authentication;
                userService.SetUserManager(userManager);
                userService.SetAuthManager(authManager);
                userService.SetRoleManager(roleManager);

                return userService;
            }
        }

        public AccountAPIController(IUserService userService, APIIModelDataMapper dataMapper)
        {
            this.userService = userService;
            this.dataMapper = dataMapper;
        }

        [Authorize(Roles = "admin")]
        [Route("api/Users")]
        public IHttpActionResult GetUsers(int pageSize, int pageNumber = 1, string searchQuery = null)
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

            PaginationDTO<UserDTO> userPaginationDTO = UserService.GetUserPagination(pageNumber, pageSize, searchQuery: searchQuery);

            if (userPaginationDTO == null)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                APIPaginationModel<APIUserModel> userPaginationModel = dataMapper.MapUserPaginationDTOToModel(userPaginationDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
                    data = userPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Users/Login")]
        public async Task<IHttpActionResult> Login([FromBody]APIUserLoginModel userLoginModel)
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

            string userName = userLoginModel.UserName;
            string password = userLoginModel.Password;
            bool isPersistent = userLoginModel.IsPersistent;
            IdentityResult result = await UserService.LoginAsAdmin(userName, password, isPersistent);

            jsonObject = new
            {
                status = HttpStatusCode.OK,
                data = result.Succeeded
            };

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Users/Logout")]
        public IHttpActionResult Logout()
        {
            object jsonObject;
            AuthDTO authDTO = UserService.GetAuth();

            if (!authDTO.IsAuthenticated)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                UserService.Logout();

                jsonObject = new
                {
                    status = HttpStatusCode.OK
                };
            }

            return Json(jsonObject);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/Users/Lockout/{userName}")]
        public async Task<IHttpActionResult> Lockout(string userName, bool lockout)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(userName))
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            IdentityResult result = await UserService.SetLockout(userName, lockout);

            if (!result.Succeeded)
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/Users/Admin/{userName}")]
        public async Task<IHttpActionResult> AddAdmin(string userName)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(userName))
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            IdentityResult result = await UserService.AddAdmin(userName);

            if (!result.Succeeded)
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
