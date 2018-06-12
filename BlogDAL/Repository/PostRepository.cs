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
            return base.Add(entity);
        }

        public PostEntity GetPostEntityWithPagination(string id)
        {
            try
            {
                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(id))
                    .First();

                List<CommentEntity> commentEntities = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(id))
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(pagination.CommentPageSize)
                    .ToList();

                postEntity.CommentEntities = commentEntities;

                return postEntity;
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
                    .Where(x => x.PostCategory.Equals(category))
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
