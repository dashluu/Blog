using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public interface ICommentRepository : IBaseRepository<CommentEntity>
    {
        List<CommentEntity> GetChildComments(string commentId);
        PaginationEntity<CommentEntity> GetCommentPaginationOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize);
        PaginationEntity<CommentEntity> GetCommentPagination(int pageNumber, int pageSize, string postId = null, string commentId = null, string searchQuery = null);

        bool Remove(string commentId);
    }
}
