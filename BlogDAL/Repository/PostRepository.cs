﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BlogDAL.Entity;

namespace BlogDAL.Repository
{
    public class PostRepository : BaseRepository<PostEntity, BlogDBContext>, IPostRepository
    {
        private ICommentRepository commentRepository;

        public PostRepository(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public override bool Add(PostEntity entity)
        {
            entity.PostId = GenerateId();
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            CategoryEntity categoryEntity = Context.CategoryEntities
                .Where(x => x.Name.Equals(entity.PostCategory.Name))
                .First();

            categoryEntity.PostCount++;

            entity.PostCategory = categoryEntity;

            return base.Add(entity);
        }

        public PostEntityWithPaginatedComments GetPostEntityWithPaginatedComments(string id, int pageSize)
        {
            try
            {
                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(id))
                    .Include(x => x.PostCategory)
                    .First();

                PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPaginationEntityOfPostWithPreservedFetch(postId: id, DateTime.Now, pageSize);

                PostEntityWithPaginatedComments postEntityWithPaginatedComments = new PostEntityWithPaginatedComments()
                {
                    Post = postEntity,
                    CommentPaginationEntity = commentPaginationEntity
                };

                return postEntityWithPaginatedComments;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<PostEntity> GetPostPaginationEntity(string category, int pageNumber, int pageSize)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;

                IQueryable<PostEntity> postQueryable = Context.PostEntities
                    .Include(x => x.PostCategory);

                if (!string.IsNullOrWhiteSpace(category))
                {
                    postQueryable = postQueryable.Where(x => x.PostCategory.Name.Equals(category));
                }

                Expression<Func<PostEntity, DateTime>> postOrderByExpression = (x => x.CreatedDate);

                PaginationEntity<PostEntity> postPaginationEntity = GetPaginationEntity(postQueryable, isDesc: true, postOrderByExpression, skip, pageSize);

                return postPaginationEntity;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public List<PaginationEntity<PostEntity>> GetPostPaginationEntities(int pageSize)
        {
            try
            {
                List<CategoryEntity> categoryEntities = Context.CategoryEntities.ToList();
                List<PaginationEntity<PostEntity>> postPaginationEntities = new List<PaginationEntity<PostEntity>>();

                foreach (CategoryEntity categoryEntity in categoryEntities)
                {
                    PaginationEntity<PostEntity> postPaginationEntity = GetPostPaginationEntity(category: categoryEntity.Name, pageNumber: 1, pageSize);

                    if (postPaginationEntity == null || postPaginationEntity.Entities == null || postPaginationEntity.Pages == 0)
                    {
                        continue;
                    }

                    postPaginationEntities.Add(postPaginationEntity);
                }

                return postPaginationEntities;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public PostEntity GetPostEntity(string id)
        {
            try
            {
                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(id))
                    .Include(x => x.PostCategory)
                    .First();

                return postEntity;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public PaginationEntity<PostEntity> RemovePostEntityWithReloadedPagination(string category, string postId, int pageNumber, int pageSize)
        {
            try
            {
                DbSet<PostEntity> postEntityDbSet = Context.PostEntities;
                DbSet<CommentEntity> commentEntityDbSet = Context.CommentEntities;

                //Get child comments that belong to post.
                IQueryable<CommentEntity> commentDeleteQueryable = commentEntityDbSet
                    .Where(x => x.Post.PostId.Equals(postId) && x.RootComment != null);

                List<CommentEntity> commentEntities = commentDeleteQueryable.ToList();

                //Remove child comments that belong to post.
                commentEntityDbSet.RemoveRange(commentEntities);

                //Get comments that belong to post.
                commentDeleteQueryable = commentEntityDbSet
                    .Where(x => x.Post.PostId.Equals(postId));

                commentEntities = commentDeleteQueryable.ToList();

                //Remove comments that belong to post.
                commentEntityDbSet.RemoveRange(commentEntities);

                //Get and remove post.
                PostEntity postEntity = postEntityDbSet.Where(x => x.PostId.Equals(postId)).First();
                postEntityDbSet.Remove(postEntity);

                Context.SaveChanges();

                //Get and return post pagination.
                IQueryable<PostEntity> postPaginationQueryable = postEntityDbSet.AsQueryable();

                if (!string.IsNullOrWhiteSpace(category))
                {
                    postPaginationQueryable = postPaginationQueryable
                        .Where(x => x.PostCategory.Name.Equals(category));
                }

                Expression<Func<PostEntity, DateTime>> postOrderByExpression = (x => x.CreatedDate);
                int skip = (pageNumber - 1) * pageSize;

                PaginationEntity<PostEntity> postPaginationEntity = GetPaginationEntity
                    (postPaginationQueryable,
                    isDesc: true,
                    postOrderByExpression,
                    skip,
                    pageSize);

                return postPaginationEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }
    }
}
