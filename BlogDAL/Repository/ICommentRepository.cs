using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public interface ICommentRepository : IBaseRepository<CommentEntity>
    {
        CommentEntity AddCommentEntity(string postId, CommentEntity commentEntity);
        CommentEntity AddChildCommentEntity(string postId, string commentId, CommentEntity childCommentEntity);

        List<CommentEntity> GetChildCommentEntities(string commentId, int skip);
        PaginationEntity<CommentEntity> GetChildCommentPaginationEntity(string commentId, int pageNumber, int pageSize);

        PaginationEntity<CommentEntity> GetCommentPaginationEntityOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize);
        PaginationEntity<CommentEntity> GetCommentPaginationEntity(string postId, int pageNumber, int pageSize);

        PaginationEntity<CommentEntity> SearchCommentWithPaginationEntity(string postId, string searchQuery, int pageNumber, int pageSize);

        PaginationEntity<CommentEntity> RemoveCommentEntityWithReloadedPagination(string postId, string commentId, int pageNumber, int pageSize);
    }
}
