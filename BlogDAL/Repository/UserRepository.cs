using BlogDAL.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BlogDAL.Repository
{
    public class UserRepository : BaseRepository<UserEntity, BlogDBContext>, IUserRepository
    {
        public PaginationEntity<UserEntity> GetUserPagination(IQueryable<UserEntity> userQueryable, int pageNumber, int pageSize, string searchQuery = null)
        {
            try
            {
                userQueryable = userQueryable.Include(x => x.Roles);
                Expression<Func<UserEntity, string>> userOrderByExpression = (x => x.UserName);            

                if (searchQuery != null)
                {
                    userQueryable = userQueryable
                        .Where(x => x.UserName.Contains(searchQuery)
                        || x.Email.Contains(searchQuery));
                }

                PaginationEntity<UserEntity> userPaginationEntity = GetPaginationEntity(userQueryable, isDesc: false, userOrderByExpression, pageNumber, pageSize);

                return userPaginationEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
