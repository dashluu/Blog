using BlogDAL.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogDAL.Repository
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        PaginationEntity<UserEntity> GetUserPagination(IQueryable<UserEntity> userQueryable, int pageNumber, int pageSize, string searchQuery = null);
    }
}
