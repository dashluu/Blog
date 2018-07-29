using BlogDAL.Entity;
using BlogServices.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BlogServices.Services
{
    public interface IUserService
    {
        void SetUserManager(ServiceUserManager userManager);
        void SetAuthManager(IAuthenticationManager authManager);
        void SetRoleManager(ServiceRoleManager roleManager);
        Task<IdentityResult> Create(UserDTO userDTO);
        Task<IdentityResult> Login(string userName, string password);
        Task<IdentityResult> LoginAsAdmin(string userName, string password);
        IdentityResult Logout();
        PaginationDTO<UserDTO> GetUserPagination(int pageNumber, int pageSize, string searchQuery = null);
        Task<IdentityResult> SetLockout(string userName, bool lockout);
        Task<bool> LockoutEnabledAsync(string userName);
        bool LockoutEnabled(string userName);
        AuthDTO GetAuth();
        Task<IdentityResult> AddAdmin(string userName);
        Task<IdentityResult> UpdatePassword(string userName, string currentPassword, string newPassword);
        Task<UserDTO> GetUser(string userName);
    }
}
