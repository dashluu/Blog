using BlogDAL.Entity;
using BlogDAL.Repository;
using BlogServices.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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
        private ServiceRoleManager roleManager;

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

        public PaginationDTO<UserDTO> GetUserPagination(int pageNumber, int pageSize, string searchQuery = null)
        {
            IQueryable<UserEntity> userQueryable = userManager.GetUserQueryable();
            RoleEntity adminRoleEntity = roleManager.FindByName("admin");
            PaginationEntity<UserEntity> userPaginationEntity = userRepository.GetUserPagination(userQueryable, pageNumber, pageSize, searchQuery);
            PaginationDTO<UserDTO> userPaginationDTO = dataMapper.MapUserPaginationEntityToDTO(userPaginationEntity, adminRoleEntity);

            return userPaginationDTO;
        }

        public async Task<IdentityResult> Login(string userName, string password)
        {
            UserEntity userEntity = await userManager.FindAsync(userName, password);

            if (userEntity == null)
            {
                return IdentityResult.Failed("User or password not found.");
            }

            if (userEntity.LockoutEnabled)
            {
                return IdentityResult.Failed("User has been locked out.");
            }

            AuthenticationProperties authProperties = new AuthenticationProperties()
            {
                IsPersistent = false
            };

            ClaimsIdentity claimsIdentity = await userManager.CreateIdentityAsync(userEntity, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignOut();
            authManager.SignIn(authProperties, claimsIdentity);

            return IdentityResult.Success;
        }

        public IdentityResult Logout()
        {
            IIdentity identity = authManager.User.Identity;
            
            if (!identity.IsAuthenticated)
            {
                return IdentityResult.Failed("User is not logged in.");
            }

            authManager.SignOut();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> SetLockout(string userName, bool lockout)
        {
            UserEntity userEntity = await userManager.FindByNameAsync(userName);
            
            if (userEntity == null)
            {
                return IdentityResult.Failed("Username not found.");
            }

            bool isAdmin = await userManager.IsInRoleAsync(userEntity.Id, "admin");

            if (isAdmin)
            {
                return IdentityResult.Failed("Admin cannot be locked out.");
            }

            IdentityResult result = await userManager.SetLockoutEnabledAsync(userEntity.Id, lockout);

            return result;
        }

        public async Task<bool> LockoutEnabledAsync(string userName)
        {
            UserEntity userEntity = await userManager.FindByNameAsync(userName);

            if (userEntity == null)
            {
                return true;
            }

            bool lockout = await userManager.GetLockoutEnabledAsync(userEntity.Id);

            return lockout;
        }

        public bool LockoutEnabled(string userName)
        {
            UserEntity userEntity = userManager.FindByName(userName);

            if (userEntity == null)
            {
                return true;
            }

            bool lockout = userManager.GetLockoutEnabled(userEntity.Id);

            return lockout;
        }

        public AuthDTO GetAuth()
        {
            IIdentity identity = authManager.User.Identity;

            AuthDTO authDTO = new AuthDTO()
            {
                IsAuthenticated = identity.IsAuthenticated,
                UserName = identity.Name
            };

            return authDTO;
        }

        public void SetRoleManager(ServiceRoleManager roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> LoginAsAdmin(string userName, string password)
        {
            UserEntity userEntity = await userManager.FindAsync(userName, password);

            if (userEntity == null)
            {
                return IdentityResult.Failed("User or password not found.");
            }

            bool isAdmin = await userManager.IsInRoleAsync(userEntity.Id, "admin");

            if (!isAdmin)
            {
                return IdentityResult.Failed("User or password not found.");
            }

            AuthenticationProperties authProperties = new AuthenticationProperties()
            {
                IsPersistent = false
            };

            ClaimsIdentity claimsIdentity = await userManager.CreateIdentityAsync(userEntity, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignOut();
            authManager.SignIn(authProperties, claimsIdentity);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> AddAdmin(string userName)
        {
            UserEntity userEntity = await userManager.FindByNameAsync(userName);

            if (userEntity == null)
            {
                return IdentityResult.Failed("User not found.");
            }

            if (userEntity.LockoutEnabled)
            {
                return IdentityResult.Failed("User has been locked out.");
            }

            IdentityResult result = await userManager.AddToRoleAsync(userEntity.Id, "admin");

            return result;
        }

        public async Task<IdentityResult> UpdatePassword(string userName, string currentPassword, string newPassword)
        {
            UserEntity userEntity = await userManager.FindByNameAsync(userName);

            if (userEntity == null)
            {
                return IdentityResult.Failed("User not found.");
            }

            PasswordVerificationResult verificationResult = userManager.PasswordHasher.VerifyHashedPassword(userEntity.PasswordHash, currentPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return IdentityResult.Failed("Password not matched.");
            }

            IdentityResult identityResult = await userManager.PasswordValidator.ValidateAsync(newPassword);

            if (!identityResult.Succeeded)
            {
                return identityResult;
            }

            userEntity.PasswordHash = userManager.PasswordHasher.HashPassword(newPassword);
            identityResult = await userManager.UpdateAsync(userEntity);

            return identityResult;
        }

        public async Task<UserDTO> GetUser(string userName)
        {
            UserEntity userEntity = await userManager.FindByNameAsync(userName);
            UserDTO userDTO = dataMapper.MapUserEntityToDTO(userEntity);

            return userDTO;
        }
    }
}
