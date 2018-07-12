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
        public override bool Add(CommentEntity commentEntity)
        {
            try
            {
                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(commentEntity.PostId))
                    .First();

                postEntity.CommentCount++;

                Context.CommentEntities.Add(commentEntity);
                Context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<CommentEntity> GetChildCommentEntities(string commentId, int skip)
        {
            try
            {
                List<CommentEntity> commentEntities = Context.CommentEntities
                    .Where(x => x.ParentComment.CommentId.Equals(commentId))
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
                    && x.ParentComment == null);

                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, pageNumber: 1, pageSize);

                return commentPaginationEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> GetCommentPaginationEntity(int pageNumber, int pageSize, string postId = null)
        {
            try
            {
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities.AsQueryable();

                if (postId != null)
                {
                    commentQueryable = commentQueryable.Where(x => x.Post.PostId.Equals(postId));
                }

                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, pageNumber, pageSize);

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
                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities.Where(x => x.ParentComment.CommentId.Equals(commentId));
                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, pageNumber, pageSize);

                return commentPaginationEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> SearchCommentWithPaginationEntity(string searchQuery, int pageNumber, int pageSize, string postId = null)
        {
            try
            {
                IQueryable<CommentEntity> commentQueryable = Context.CommentEntities
                    .Where(x => x.Username.Contains(searchQuery) || x.Content.Contains(searchQuery));

                if (postId != null)
                {
                    commentQueryable = commentQueryable.Where(x => x.Post.PostId.Equals(postId));
                }

                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);
                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity(commentQueryable, isDesc: true, commentOrderByExpression, pageNumber, pageSize);

                return commentPaginationEntity;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public PaginationEntity<CommentEntity> RemoveCommentEntityWithReloadedPagination(string commentId, int pageNumber, int pageSize, string postId = null)
        {
            try
            {
                DbSet<CommentEntity> commentEntityDbSet = Context.CommentEntities;

                IQueryable<CommentEntity> commentDeleteQueryable = commentEntityDbSet
                    .Where(x => x.ParentComment.CommentId.Equals(commentId));

                List<CommentEntity> commentEntities = commentDeleteQueryable.ToList();

                CommentEntity commentEntity = commentEntityDbSet
                    .Include(x => x.Post)
                    .Include(x => x.ParentComment)
                    .Where(x => x.CommentId.Equals(commentId))
                    .First();

                commentEntities.Add(commentEntity);

                commentEntity.Post.CommentCount -= commentEntities.Count;

                commentEntityDbSet.RemoveRange(commentEntities);

                Context.SaveChanges();

                IQueryable<CommentEntity> commentPaginationQueryable = commentEntityDbSet.AsQueryable();

                if (postId != null)
                {
                    commentPaginationQueryable = commentPaginationQueryable
                        .Where(x => x.Post.PostId.Equals(postId));
                }

                Expression<Func<CommentEntity, DateTime>> commentOrderByExpression = (x => x.CreatedDate);

                PaginationEntity<CommentEntity> commentPaginationEntity = GetPaginationEntity
                    (commentPaginationQueryable,
                    isDesc: true,
                    commentOrderByExpression,
                    pageNumber,
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
