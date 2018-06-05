using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public interface IBlogRepository
    {
        List<PostEntity> GetPostEntities();
        List<PostEntity> GetPostEntitiesWithCategory(string category);
        PostEntity GetPostEntity(string id);
        string AddCommentEntity(string postId, string commentContent, string username);
        bool AddChildCommentEntity(string postId, string commentId, string commentContent, string username);
    }
}
