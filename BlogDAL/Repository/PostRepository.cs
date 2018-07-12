using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            try
            {
                CategoryEntity categoryEntity = entity.PostCategory;
                categoryEntity.PostCount++;
                entity.PostCategory = null;

                Context.CategoryEntities.Attach(categoryEntity);
                DbEntityEntry<CategoryEntity> categoryEntry = Context.Entry(categoryEntity);

                categoryEntry.Property(x => x.PostCount).IsModified = true;
                Context.PostEntities.Add(entity);

                Context.SaveChanges();
                categoryEntry.State = EntityState.Detached;

                return true;
            }
            catch(Exception)
            {
                return false;
            }
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
                IQueryable<PostEntity> postQueryable = Context.PostEntities
                    .Include(x => x.PostCategory);

                if (!string.IsNullOrWhiteSpace(category))
                {
                    postQueryable = postQueryable.Where(x => x.PostCategory.Name.Equals(category));
                }

                Expression<Func<PostEntity, DateTime>> postOrderByExpression = (x => x.CreatedDate);
                PaginationEntity<PostEntity> postPaginationEntity = GetPaginationEntity(postQueryable, isDesc: true, postOrderByExpression, pageNumber, pageSize);

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

                PaginationEntity<PostEntity> postPaginationEntity = GetPaginationEntity
                    (postPaginationQueryable,
                    isDesc: true,
                    postOrderByExpression,
                    pageNumber,
                    pageSize);

                return postPaginationEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }

        public override bool Update(PostEntity entity)
        {
            try
            {
                CategoryEntity categoryEntity = entity.PostCategory;
                categoryEntity.PostCount++;
                entity.PostCategory = null;

                Context.PostEntities.Attach(entity);
                DbEntityEntry<PostEntity> postEntry = Context.Entry(entity);
                Context.CategoryEntities.Attach(categoryEntity);
                DbEntityEntry<CategoryEntity> categoryEntry = Context.Entry(categoryEntity);

                postEntry.Property(x => x.Title).IsModified = true;
                postEntry.Property(x => x.ThumbnailImageSrc).IsModified = true;
                postEntry.Property(x => x.ShortDescription).IsModified = true;
                postEntry.Property(x => x.Content).IsModified = true;
                postEntry.Property(x => x.CategoryId).IsModified = true;
                postEntry.Property(x => x.UpdatedDate).IsModified = true;

                categoryEntry.Property(x => x.PostCount).IsModified = true;

                Context.SaveChanges();
                postEntry.State = EntityState.Detached;
                categoryEntry.State = EntityState.Detached;

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
        }
    }
}
