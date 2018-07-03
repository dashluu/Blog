using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Z.EntityFramework.Plus;

namespace BlogDAL.Repository
{
    public class CommentRepository : BaseRepository<CommentEntity, BlogDBContext>, ICommentRepository
    {
        public string AddCommentEntity(string postId, CommentEntity commentEntity)
        {
            try
            {
                string commentId = GenerateId();
                commentEntity.CommentId = commentId;
                commentEntity.CreatedDate = DateTime.Now;

                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(postId))
                    .First();

                postEntity.CommentCount++;
                commentEntity.Post = postEntity;

                Context.CommentEntities.Add(commentEntity);
                Context.SaveChanges();

                return commentId;
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
                
                CommentEntity commentEntity = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(postId) && x.CommentId.Equals(commentId))
                    .First();

                childCommentEntity.RootComment = commentEntity;

                Context.CommentEntities.Add(childCommentEntity);
                Context.SaveChanges();

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

        public PaginationEntity<CommentEntity> GetCommentPaginationEntityWithPost(string postId, int commentCount, int skip, int pageSize)
        {
            try
            {
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);

                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(postId));

                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntityWithPreservedFetch(commentQueryable, isDesc: true, commentOrderByExpression, count: commentCount, skip, pageSize);

                return commentPaginationEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> GetCommentPaginationEntity(int pageNumber, int pageSize)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities.AsQueryable();
                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, skip, pageSize);

                return commentPaginationEntity;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> GetChildCommentPaginationEntity(string commentId, int pageNumber, int pageSize)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities.Where(x => x.RootComment.CommentId.Equals(commentId));
                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, skip, pageSize);

                return commentPaginationEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> SearchCommentWithPaginationEntity(string searchQuery, int pageNumber, int pageSize)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;
                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities
                    .Where(x => x.Username.Contains(searchQuery) || x.Content.Contains(searchQuery));
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, skip, pageSize);

                return commentPaginationEntity;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
