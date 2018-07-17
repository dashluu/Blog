using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BlogDAL.Repository
{
    public interface IPostRepository : IBaseRepository<PostEntity>
    {
        PostEntity GetPost(string id);
        PostEntityWithPaginatedComments GetPostWithPaginatedComments(string id, int pageSize);
        PaginationEntity<PostEntity> GetPostPagination(int pageNumber, int pageSize, string category = null, string searchQuery = null);
        List<PaginationEntity<PostEntity>> GetPostPaginationList(int pageSize, string searchQuery = null);

        bool Remove(string postId);
    }
}
