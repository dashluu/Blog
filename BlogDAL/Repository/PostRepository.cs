using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BlogDAL.Entity;

namespace BlogDAL.Repository
{
    public class PostRepository : BaseRepository<PostEntity, BlogDBContext>, IPostRepository
    {
        private Pagination pagination;

        public PostRepository(Pagination pagination)
        {
            this.pagination = pagination;
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

        public (PostEntity postEntity, bool end) GetPostEntityWithCommentPagination(string id)
        {
            try
            {
                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(id))
                    .Include(x => x.PostCategory)
                    .First();

                IQueryable<CommentEntity> commentQuery = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(id));

                List<CommentEntity> commentEntities = commentQuery
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(pagination.CommentPageSize)
                    .ToList();

                int countComment = commentQuery.Count();
                bool end = countComment <= pagination.CommentPageSize;

                postEntity.CommentEntities = commentEntities;

                return (postEntity, end);
            }
            catch (Exception)
            {
                return (null, false);
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
    }
}
