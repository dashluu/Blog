using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
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
                Context.PostEntities.Attach(entity);
                DbEntityEntry<CategoryEntity> categoryEntry = Context.Entry(categoryEntity);
                DbEntityEntry<PostEntity> postEntry = Context.Entry(entity);

                categoryEntry.Property(x => x.PostCount).IsModified = true;
                postEntry.State = EntityState.Added;

                Context.SaveChanges();

                categoryEntry.State = EntityState.Detached;
                postEntry.State = EntityState.Detached;

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
                    .AsNoTracking()
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

        public PaginationEntity<PostEntity> GetPostPaginationEntity(int pageNumber, int pageSize, string category = null, string searchQuery = null)
        {
            try
            {
                IQueryable<PostEntity> postQueryable = Context.PostEntities
                    .Include(x => x.PostCategory);

                if (category != null)
                {
                    postQueryable = postQueryable
                        .Where(x => x.PostCategory.Name.Equals(category));
                }

                if (searchQuery != null)
                {
                    string encodedSearchQuery = HttpUtility.HtmlEncode(searchQuery);

                    postQueryable = postQueryable
                        .Where(x => x.Title.Contains(encodedSearchQuery)
                        || x.ShortDescription.Contains(encodedSearchQuery)
                        || x.PostCategory.Name.Contains(encodedSearchQuery));
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

        public List<PaginationEntity<PostEntity>> GetPostPaginationEntities(int pageSize, string searchQuery = null)
        {
            try
            {
                List<CategoryEntity> categoryEntities = Context.CategoryEntities.ToList();
                List<PaginationEntity<PostEntity>> postPaginationEntities = new List<PaginationEntity<PostEntity>>();

                foreach (CategoryEntity categoryEntity in categoryEntities)
                {
                    PaginationEntity<PostEntity> postPaginationEntity = GetPostPaginationEntity(pageNumber: 1, pageSize, category: categoryEntity.Name, searchQuery);

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
                    .AsNoTracking()
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

        public bool Remove(string postId)
        {
            try
            {
                DbSet<PostEntity> postEntityDbSet = Context.PostEntities;
                DbSet<CommentEntity> commentEntityDbSet = Context.CommentEntities;

                //Get child comments that belong to post.
                IQueryable<CommentEntity> commentDeleteQueryable = commentEntityDbSet
                    .Where(x => x.Post.PostId.Equals(postId) && x.ParentCommentId != null);

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
                PostEntity postEntity = postEntityDbSet
                    .Include(x => x.PostCategory)
                    .Where(x => x.PostId.Equals(postId))
                    .First();

                CategoryEntity categoryEntity = postEntity.PostCategory;
                categoryEntity.PostCount--;

                postEntityDbSet.Remove(postEntity);

                Context.SaveChanges();

                Context.Entry(categoryEntity).State = EntityState.Detached;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Update(PostEntity entity)
        {
            try
            {
                CategoryEntity categoryEntity = entity.PostCategory;
                entity.PostCategory = null;

                CategoryEntity oldCategoryEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(entity.PostId))
                    .Include(x => x.PostCategory)
                    .Select(x => x.PostCategory)
                    .First();

                DbEntityEntry<CategoryEntity> categoryEntry = null;

                if (!categoryEntity.CategoryId.Equals(oldCategoryEntity.CategoryId))
                {
                    categoryEntity.PostCount++;
                    oldCategoryEntity.PostCount--;
                    Context.CategoryEntities.Attach(categoryEntity);
                    categoryEntry = Context.Entry(categoryEntity);
                    categoryEntry.Property(x => x.PostCount).IsModified = true;
                }

                Context.PostEntities.Attach(entity);
                DbEntityEntry<PostEntity> postEntry = Context.Entry(entity);

                postEntry.Property(x => x.Title).IsModified = true;
                postEntry.Property(x => x.ThumbnailImageSrc).IsModified = true;
                postEntry.Property(x => x.ShortDescription).IsModified = true;
                postEntry.Property(x => x.Content).IsModified = true;
                postEntry.Property(x => x.CategoryId).IsModified = true;
                postEntry.Property(x => x.UpdatedDate).IsModified = true;

                Context.SaveChanges();

                postEntry.State = EntityState.Detached;
                Context.Entry(oldCategoryEntity).State = EntityState.Detached;

                if (categoryEntry != null)
                {
                    categoryEntry.State = EntityState.Detached;
                }

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
