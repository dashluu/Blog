using BlogDAL.Entity;
using BlogServices.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogServices.Services
{
    public interface IUserService
    {
        void SetUserManager(ServiceUserManager userManager);
        void SetAuthManager(IAuthenticationManager authManager);
        Task<IdentityResult> Create(UserDTO userDTO);
        Task<UserDTO> Find(string userName, string password);
        Task<bool> LogIn(string userName, string password);
        void LogOut();
        PaginationDTO<UserDTO> GetUserPagination(int pageNumber, int pageSize, string searchQuery = null);
    }
}
