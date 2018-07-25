using Blog.Models;
using BlogDAL.Entity;
using BlogServices.DTO;
using BlogServices.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private IUserService userService;
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

        public AccountController(IUserService userService, IModelDataMapper dataMapper)
        {
            this.userService = userService;
            this.dataMapper = dataMapper;
        }

        // GET: Account
        public ActionResult LogIn(string ReturnUrl = "/")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        public ActionResult LogOut()
        {
            UserService.LogOut();
            return RedirectToAction("LogIn", "Account");
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(UserLogInModel userModel, string ReturnUrl)
        {
            string userName = userModel.UserName;
            string password = userModel.Password;
            bool logInSuccessfully = await UserService.LogIn(userName, password);

            if (!logInSuccessfully)
            {
                ModelState.AddModelError("", "Username or password not found.");
                return View(userModel);
            }

            return Redirect(ReturnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(UserSignUpModel userModel)
        {
            UserDTO userDTO = dataMapper.MapUserSignUpModelToDTO(userModel);
            IdentityResult result = await UserService.Create(userDTO);

            if (!result.Succeeded)
            {
                AddErrors(result.Errors);
                return View(userModel);
            }

            await UserService.LogIn(userDTO.UserName, userDTO.Password);

            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}