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
                ServiceRoleManager roleManager = HttpContext.GetOwinContext().GetUserManager<ServiceRoleManager>();
                IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                userService.SetUserManager(userManager);
                userService.SetRoleManager(roleManager);
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
        public async Task<ActionResult> Login(string ReturnUrl = "/")
        {
            AuthDTO authDTO = UserService.GetAuth();

            if (authDTO.IsAuthenticated)
            {
                bool lockoutEnabled = await UserService.LockoutEnabled(authDTO.UserName);

                if (lockoutEnabled)
                {
                    UserService.Logout();
                }
                else
                {
                    return Redirect(ReturnUrl);
                }
            }

            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.IsAuthenticated = false;
            ViewBag.UserName = "";

            return View();
        }

        public ActionResult Logout(string ReturnUrl = "/")
        {
            AuthDTO authDTO = UserService.GetAuth();

            if (!authDTO.IsAuthenticated)
            {
                return Redirect(ReturnUrl);
            }

            UserService.Logout();

            return RedirectToAction("Login");
        }

        public async Task<ActionResult> SignUp(string ReturnUrl = "/")
        {
            AuthDTO authDTO = UserService.GetAuth();

            if (authDTO.IsAuthenticated)
            {
                bool lockoutEnabled = await UserService.LockoutEnabled(authDTO.UserName);

                if (lockoutEnabled)
                {
                    UserService.Logout();
                }
                else
                {
                    return Redirect(ReturnUrl);
                }
            }

            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.IsAuthenticated = false;
            ViewBag.UserName = "";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginModel userModel, string ReturnUrl = "/")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.IsAuthenticated = false;
            ViewBag.UserName = "";

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid inputs.");
                return View(userModel);
            }

            AuthDTO authDTO = UserService.GetAuth();

            if (authDTO.IsAuthenticated)
            {
                bool lockoutEnabled = await UserService.LockoutEnabled(authDTO.UserName);

                if (lockoutEnabled)
                {
                    UserService.Logout();
                    ModelState.AddModelError("", "User has been locked out.");

                    return View(userModel);
                }
                else
                {
                    return Redirect(ReturnUrl);
                }
            }

            string userName = userModel.UserName;
            string password = userModel.Password;
            IdentityResult result = await UserService.Login(userName, password);

            if (result.Succeeded)
            {
                return Redirect(ReturnUrl);
            }

            AddErrors(result.Errors);

            return View(userModel);
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(UserSignUpModel userModel, string ReturnUrl = "/")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.IsAuthenticated = false;
            ViewBag.UserName = "";

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid inputs.");
                return View(userModel);
            }

            AuthDTO authDTO = UserService.GetAuth();

            if (authDTO.IsAuthenticated)
            {
                bool lockoutEnabled = await UserService.LockoutEnabled(authDTO.UserName);

                if (lockoutEnabled)
                {
                    UserService.Logout();
                    ModelState.AddModelError("", "User has been locked out.");

                    return View(userModel);
                }
                else
                {
                    return Redirect(ReturnUrl);
                }
            }

            UserDTO userDTO = dataMapper.MapUserSignUpModelToDTO(userModel);
            IdentityResult result = await UserService.Create(userDTO);

            if (result.Succeeded)
            {
                await UserService.Login(userDTO.UserName, userDTO.Password);
                return Redirect(ReturnUrl);
            }

            AddErrors(result.Errors);

            return View(userModel);
        }

        [Authorize]
        public async Task<ActionResult> Info()
        {
            AuthDTO authDTO = UserService.GetAuth();
            bool lockoutEnabled = await UserService.LockoutEnabled(authDTO.UserName);

            if (lockoutEnabled)
            {
                UserService.Logout();
                return RedirectToAction("Login");
            }

            ViewBag.IsAuthenticated = true;
            ViewBag.ReturnUrl = Request.Url.AbsolutePath;
            ViewBag.UserName = authDTO.UserName;
            UserDTO userDTO = await UserService.GetUser(authDTO.UserName);
            UserEditModel userModel = dataMapper.MapUserEditDTOToModel(userDTO);

            return View(userModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Info(UserEditModel userModel)
        {
            AuthDTO authDTO = UserService.GetAuth();
            string userName = authDTO.UserName;
            bool lockoutEnabled = await UserService.LockoutEnabled(userName);

            if (lockoutEnabled)
            {
                UserService.Logout();
                return RedirectToAction("Login");
            }

            ViewBag.IsAuthenticated = true;
            ViewBag.ReturnUrl = "/Account/Info";
            ViewBag.UserName = userName;
            UserDTO userDTO;

            if (!ModelState.IsValid)
            {
                userDTO = await UserService.GetUser(userName);
                userModel = dataMapper.MapUserEditDTOToModel(userDTO);
                ModelState.AddModelError("", "Invalid inputs.");

                return View("Info", userModel);
            }

            userDTO = dataMapper.MapUserEditModelToDTO(userModel);
            userDTO.UserName = userName;
            IdentityResult result = await UserService.UpdateInfo(userDTO);

            if (result.Succeeded)
            {
                return View("Info", userModel);
            }

            userDTO = await UserService.GetUser(userName);
            userModel = dataMapper.MapUserEditDTOToModel(userDTO);
            AddErrors(result.Errors, "Info");

            return View("Info", userModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Password(string CurrentPassword, string NewPassword)
        {
            AuthDTO authDTO = UserService.GetAuth();
            string userName = authDTO.UserName;
            bool lockoutEnabled = await UserService.LockoutEnabled(userName);

            if (lockoutEnabled)
            {
                UserService.Logout();
                return RedirectToAction("Login");
            }

            ViewBag.IsAuthenticated = true;
            ViewBag.ReturnUrl = "/Account/Info";
            ViewBag.UserName = userName;
            UserDTO userDTO = await UserService.GetUser(userName);
            UserEditModel userModel = dataMapper.MapUserEditDTOToModel(userDTO);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid inputs.");
                return View("Info", userModel);
            }

            IdentityResult result = await UserService.UpdatePassword(userName, CurrentPassword, NewPassword);

            if (result.Succeeded)
            {
                return View("Info", userModel);
            }

            AddErrors(result.Errors, "Password");

            return View("Info", userModel);
        }

        private void AddErrors(IEnumerable<string> errors, string key = "")
        {
            foreach (string error in errors)
            {
                ModelState.AddModelError(key, error);
            }
        }
    }
}