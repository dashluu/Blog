using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public interface ICommentRepository : IBaseRepository<CommentEntity>
    {
        string AddCommentEntity(string postId, CommentEntity commentEntity);
        CommentEntity GetCommentEntity(string postId, string commentId);
        bool AddChildCommentEntity(string postId, string commentId, CommentEntity childCommentEntity);
        (List<CommentEntity> commentEntities, bool end) Paginate(string postId, int skip);
        List<CommentEntity> GetChildCommentEntities(string commentId);
    }
}
