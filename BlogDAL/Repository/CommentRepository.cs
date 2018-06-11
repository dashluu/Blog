using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace BlogDAL.Repository
{
    public class CommentRepository : BaseRepository<CommentEntity, BlogDBContext>, ICommentRepository
    {
        private IPostRepository postRepository;

        public CommentRepository(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public string AddCommentEntity(string postId, CommentEntity commentEntity)
        {
            if (postId == null || commentEntity == null)
            {
                return null;
            }

            try
            {
                string commentId = GenerateId();
                commentEntity.CommentId = commentId;
                PostEntity postEntity = postRepository.GetPostEntity(postId);
                postEntity.CommentEntities.Add(commentEntity);
                bool saveSuccessfully = postRepository.SaveChanges();
                
                if (!saveSuccessfully)
                {
                    return null;
                }

                return commentId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CommentEntity GetCommentEntity(string postId, string commentId)
        {
            if (postId == null || commentId == null)
            {
                return null;
            }

            try
            {
                PostEntity postEntity = postRepository.GetPostEntity(postId);
                List<CommentEntity> commentEntities = postEntity.CommentEntities;

                foreach (CommentEntity commentEntity in commentEntities)
                {
                    if (commentEntity.CommentId.Equals(commentId))
                    {
                        return commentEntity;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AddChildCommentEntity(string postId, string commentId, CommentEntity childCommentEntity)
        {
            if (postId == null || commentId == null || childCommentEntity == null)
            {
                return false;
            }

            try
            {
                string childCommentId = GenerateId();
                childCommentEntity.CommentId = childCommentId;
                CommentEntity commentEntity = GetCommentEntity(postId, commentId);
                commentEntity.ChildCommentEntities.Add(childCommentEntity);
                bool saveSuccessfully = postRepository.SaveChanges();

                if (!saveSuccessfully)
                {
                    return false;
                }

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
