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
        PostEntity GetPost(string id);
        List<CommentEntity> GetCommentEntities(PostEntity postEntity);
        List<CommentEntity> GetCommentEntitiesWithRoot(CommentEntity commentEntity);
    }
}
