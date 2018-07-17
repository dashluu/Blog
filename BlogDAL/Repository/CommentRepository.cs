﻿using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
                //Get and track post.
                //Update number of comments in post.
                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(commentEntity.PostId))
                    .First();

                postEntity.CommentCount++;

                //Attach to track new comment.
                //Add comment.
                Context.CommentEntities.Attach(commentEntity);
                DbEntityEntry<CommentEntity> commentEntry = Context.Entry(commentEntity);
                commentEntry.State = EntityState.Added;

                Context.SaveChanges();

                //Detach post and comment to stop tracking.
                Context.Entry(postEntity).State = EntityState.Detached;
                commentEntry.State = EntityState.Detached;

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
                    .AsNoTracking()
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

        public bool Remove(string commentId)
        {
            try
            {
                DbSet<CommentEntity> commentEntityDbSet = Context.CommentEntities;

                IQueryable<CommentEntity> childCommentQueryable = commentEntityDbSet
                    .Where(x => x.ParentComment.CommentId.Equals(commentId));

                List<CommentEntity> childCommentEntities = childCommentQueryable.ToList();

                CommentEntity commentEntity = commentEntityDbSet
                    .Include(x => x.Post)
                    .Where(x => x.CommentId.Equals(commentId))
                    .First();

                PostEntity postEntity = commentEntity.Post;

                postEntity.CommentCount -= (childCommentEntities.Count + 1);
                commentEntityDbSet.RemoveRange(childCommentEntities);
                commentEntityDbSet.Remove(commentEntity);

                Context.SaveChanges();
                Context.Entry(postEntity).State = EntityState.Detached;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
