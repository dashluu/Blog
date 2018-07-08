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
        public CommentEntity AddCommentEntity(string postId, CommentEntity commentEntity)
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

                return commentEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CommentEntity AddChildCommentEntity(string postId, string commentId, CommentEntity childCommentEntity)
        {
            try
            {
                string childCommentId = GenerateId();
                childCommentEntity.CommentId = childCommentId;
                childCommentEntity.CreatedDate = DateTime.Now;
                
                CommentEntity commentEntity = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(postId) && x.CommentId.Equals(commentId))
                    .First();

                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(postId))
                    .First();

                postEntity.CommentCount++;

                childCommentEntity.RootComment = commentEntity;
                childCommentEntity.Post = postEntity;

                Context.CommentEntities.Add(childCommentEntity);
                Context.SaveChanges();

                return childCommentEntity;
            }
            catch(Exception)
            {
                return null;
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

        public PaginationEntity<CommentEntity> GetCommentPaginationEntityOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize)
        {
            try
            {
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);

                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(postId) 
                    && DateTime.Compare(x.CreatedDate, createdDate) < 0
                    && x.RootComment == null);

                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, skip: 0, pageSize);

                return commentPaginationEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> GetCommentPaginationEntity(string postId, int pageNumber, int pageSize)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities.AsQueryable();

                if (!string.IsNullOrWhiteSpace(postId))
                {
                    commentQueryable = commentQueryable.Where(x => x.Post.PostId.Equals(postId));
                }

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

        public PaginationEntity<CommentEntity> SearchCommentWithPaginationEntity(string postId, string searchQuery, int pageNumber, int pageSize)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;

                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities
                    .Where(x => x.Username.Contains(searchQuery) || x.Content.Contains(searchQuery));

                if (!string.IsNullOrWhiteSpace(postId))
                {
                    commentQueryable = commentQueryable.Where(x => x.Post.PostId.Equals(postId));
                }

                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, skip, pageSize);

                return commentPaginationEntity;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> RemoveCommentEntityWithReloadedPagination(string postId, string commentId, int pageNumber, int pageSize)
        {
            try
            {
                DbSet<CommentEntity> commentEntityDbSet = Context.CommentEntities;

                IQueryable<CommentEntity> commentDeleteQueryable = commentEntityDbSet
                    .Where(x => x.RootComment.CommentId.Equals(commentId));

                List<CommentEntity> commentEntities = commentDeleteQueryable.ToList();

                CommentEntity commentEntity = commentEntityDbSet
                    .Include(x => x.Post)
                    .Include(x => x.RootComment)
                    .Where(x => x.CommentId.Equals(commentId))
                    .First();

                commentEntities.Add(commentEntity);

                commentEntity.Post.CommentCount -= commentEntities.Count;

                commentEntityDbSet.RemoveRange(commentEntities);

                Context.SaveChanges();

                IQueryable<CommentEntity> commentPaginationQueryable = commentEntityDbSet.AsQueryable();

                if (!string.IsNullOrWhiteSpace(postId))
                {
                    commentPaginationQueryable = commentPaginationQueryable
                        .Where(x => x.Post.PostId.Equals(postId));
                }

                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                int skip = (pageNumber - 1) * pageSize;

                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity
                    (commentPaginationQueryable,
                    isDesc: true,
                    commentOrderByExpression,
                    skip,
                    pageSize);

                return commentPaginationEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }
    }
}
