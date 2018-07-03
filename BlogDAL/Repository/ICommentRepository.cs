using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public interface ICommentRepository : IBaseRepository<CommentEntity>
    {
        string AddCommentEntity(string postId, CommentEntity commentEntity);
        bool AddChildCommentEntity(string postId, string commentId, CommentEntity childCommentEntity);
        List<CommentEntity> GetChildCommentEntities(string commentId, int skip);
        PaginationEntity<CommentEntity> GetCommentPaginationEntityWithPost(string postId, int commentCount, int skip, int pageSize);
        PaginationEntity<CommentEntity> GetCommentPaginationEntity(int pageNumber, int pageSize);
        PaginationEntity<CommentEntity> GetChildCommentPaginationEntity(string commentId, int pageNumber, int pageSize);
        PaginationEntity<CommentEntity> SearchCommentWithPaginationEntity(string searchQuery, int pageNumber, int pageSize);
    }
}
