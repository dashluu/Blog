using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BlogDAL.Repository
{
    public interface IPostRepository : IBaseRepository<PostEntity>
    {
        List<PostEntity> GetPostEntitiesWithCategory(string category);
        PostEntityWithPaginatedComments GetPostEntityWithPaginatedComments(string id, int pageSize);
        PostEntity GetPostEntity(string id);
        PaginationEntity<PostEntity> GetPostPaginationEntityWithCategory(string category, int pageNumber, int pageSize);
        List<PaginationEntity<PostEntity>> GetPostPaginationEntities(int pageSize);
    }
}
