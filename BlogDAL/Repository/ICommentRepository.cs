using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public interface ICommentRepository : IBaseRepository<CommentEntity>
    {
        List<CommentEntity> GetChildCommentEntities(string commentId, int skip);
        PaginationEntity<CommentEntity> GetChildCommentPaginationEntity(string commentId, int pageNumber, int pageSize);

        PaginationEntity<CommentEntity> GetCommentPaginationEntityOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize);
        PaginationEntity<CommentEntity> GetCommentPaginationEntity(int pageNumber, int pageSize, string postId = null);

        PaginationEntity<CommentEntity> SearchCommentWithPaginationEntity(string searchQuery, int pageNumber, int pageSize, string postId = null);

        PaginationEntity<CommentEntity> RemoveCommentEntityWithReloadedPagination(string commentId, int pageNumber, int pageSize, string postId = null);
    }
}
