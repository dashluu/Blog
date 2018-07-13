using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BlogDAL.Repository
{
    public interface IPostRepository : IBaseRepository<PostEntity>
    {
        PostEntity GetPostEntity(string id);
        PostEntityWithPaginatedComments GetPostEntityWithPaginatedComments(string id, int pageSize);
        PaginationEntity<PostEntity> GetPostPaginationEntity(int pageNumber, int pageSize, string category = null);
        List<PaginationEntity<PostEntity>> GetPostPaginationEntities(int pageSize);

        bool Remove(string postId);
    }
}
