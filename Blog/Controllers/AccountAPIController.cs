using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
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
                IAuthenticationManager authManager = httpContext.GetOwinContext().Authentication;
                userService.SetUserManager(userManager);
                userService.SetAuthManager(authManager);

                return userService;
            }
        }

        public AccountAPIController(IUserService userService, APIIModelDataMapper dataMapper)
        {
            this.userService = userService;
            this.dataMapper = dataMapper;
        }

        [Route("api/Users")]
        public IHttpActionResult GetUsers(int pageSize, int pageNumber = 1, string searchQuery = null)
        {
            object jsonObject;

            if (pageNumber <= 0 || pageSize < 0)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            PaginationDTO<UserDTO> userPaginationDTO = UserService.GetUserPagination(pageNumber, pageSize, searchQuery: searchQuery);

            if (userPaginationDTO == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                APIPaginationModel<APIUserModel> userPaginationModel = dataMapper.MapUserPaginationDTOToModel(userPaginationDTO);

                jsonObject = new
                {
                    status = 200,
                    data = userPaginationModel
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Users/Login")]
        public async Task<IHttpActionResult> Login([FromBody]APIUserLogInModel userLogInModel)
        {
            object jsonObject;

            if (!ModelState.IsValid)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            bool logInSuccessfully = await UserService.LogIn(userLogInModel.UserName, userLogInModel.Password);

            jsonObject = new {
                status = 200,
                data = logInSuccessfully
            };

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Users/Logout")]
        public IHttpActionResult Logout()
        {
            UserService.LogOut();
            return Json(new { });
        }
    }
}
