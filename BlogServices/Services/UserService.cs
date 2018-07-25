using BlogDAL.Entity;
using BlogDAL.Repository;
using BlogServices.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogServices.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private IHashService hashService;
        private IServiceDataMapper dataMapper;
        private ServiceUserManager userManager;
        private IAuthenticationManager authManager;

        public UserService(IUserRepository userRepository, IServiceDataMapper dataMapper, IHashService hashService)
        {
            this.userRepository = userRepository;
            this.dataMapper = dataMapper;
            this.hashService = hashService;
        }

        public void SetUserManager(ServiceUserManager userManager)
        {
            this.userManager = userManager;
        }

        public void SetAuthManager(IAuthenticationManager authManager)
        {
            this.authManager = authManager;
        }

        public async Task<IdentityResult> Create(UserDTO userDTO)
        {
            userDTO.Id = hashService.GenerateId();

            UserEntity userEntity = dataMapper.MapUserDTOToEntity(userDTO);
            IdentityResult result = await userManager.CreateAsync(userEntity, userDTO.Password);
            
            return result;
        }

        public async Task<UserDTO> Find(string userName, string password)
        {
            UserEntity userEntity = await userManager.FindAsync(userName, password);
            UserDTO userDTO = dataMapper.MapUserEntityToDTO(userEntity);

            return userDTO;
        }

        public PaginationDTO<UserDTO> GetUserPagination(int pageNumber, int pageSize, string searchQuery = null)
        {
            IQueryable<UserEntity> userQueryable = userManager.GetUserQueryable();
            PaginationEntity<UserEntity> userPaginationEntity = userRepository.GetUserPagination(userQueryable, pageNumber, pageSize, searchQuery);
            PaginationDTO<UserDTO> userPaginationDTO = dataMapper.MapUserPaginationEntityToDTO(userPaginationEntity);

            return userPaginationDTO;
        }

        public async Task<bool> LogIn(string userName, string password)
        {
            UserEntity userEntity = userManager.Find(userName, password);

            if (userEntity == null)
            {
                return false;
            }

            AuthenticationProperties authProperties = new AuthenticationProperties()
            {
                IsPersistent = false
            };

            ClaimsIdentity claimsIdentity = await userManager.CreateIdentityAsync(userEntity, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignOut();
            authManager.SignIn(authProperties, claimsIdentity);

            return true;
        }

        public void LogOut()
        {
            authManager.SignOut();
        }
    }
}
