using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BlogDAL.Repository
{
    public class CommentRepository : BaseRepository<CommentEntity, BlogDBContext>, ICommentRepository
    {
        private string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public string AddCommentEntity(string postId, CommentEntity commentEntity)
        {
            try
            {
                string commentId = GenerateId();
                commentEntity.CommentId = commentId;
                commentEntity.CreatedDate = DateTime.Now;
                commentEntity.Post = Context.PostEntities
                    .Where(x => x.PostId.Equals(postId))
                    .First();
                bool addSuccessfully = Add(commentEntity);

                if (!addSuccessfully)
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
            try
            {
                CommentEntity commentEntity = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(postId) && x.CommentId.Equals(commentId))
                    .First();

                return commentEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AddChildCommentEntity(string postId, string commentId, CommentEntity childCommentEntity)
        {
            try
            {
                string childCommentId = GenerateId();
                childCommentEntity.CommentId = childCommentId;
                childCommentEntity.CreatedDate = DateTime.Now;
                childCommentEntity.RootComment = GetCommentEntity(postId, commentId);
                bool addSuccessfully = Add(childCommentEntity);

                if (!addSuccessfully)
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

        public List<CommentEntity> GetChildCommentEntities(string commentId, int skip)
        {
            try
            {
                List<CommentEntity> commentEntities = Context.CommentEntities
                    .Where(x => x.RootComment.CommentId.Equals(commentId))
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip(skip)
                    .ToList();

                return commentEntities;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> GetCommentPaginationEntityWithPost(string postId, int skip, int pageSize)
        {
            try
            {
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);

                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(postId));

                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, skip, pageSize);

                return commentPaginationEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
