using System;
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

        private string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public override bool Add(PostEntity entity)
        {
            entity.PostId = GenerateId();
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            CategoryEntity categoryEntity = Context.CategoryEntities
                .Where(x => x.Name.Equals(entity.PostCategory.Name))
                .First();

            categoryEntity.Statistics++;

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

                PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPaginationEntityWithPost(postId: id, skip: 0, pageSize);

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

        public List<PostEntity> GetPostEntitiesWithCategory(string category)
        {
            try
            {
                List<PostEntity> postEntities = Context.PostEntities
                    .Where(x => x.PostCategory.Name.Equals(category))
                    .Include(x => x.PostCategory)
                    .ToList();
                return postEntities;
            }
            catch (Exception)
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
                    .First();

                return postEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PaginationEntity<PostEntity> GetPostPaginationEntityWithCategory(string category, int pageNumber, int pageSize)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;

                IQueryable<PostEntity> postQueryable = Context.PostEntities
                    .Where(x => x.PostCategory.Name.Equals(category))
                    .Include(x => x.PostCategory);

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
                    PaginationEntity<PostEntity> postPaginationEntity = GetPostPaginationEntityWithCategory(category: categoryEntity.Name, pageNumber: 1, pageSize);

                    if (postPaginationEntity == null)
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
    }
}
