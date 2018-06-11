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

        public PostEntity GetPostEntity(string id)
        {
            try
            {
                PostEntity postEntity = Context.PostEntities
                    .Where(x => x.PostId.Equals(id))
                    .Include(y => y.CommentEntities
                                   .Select(z => z.ChildCommentEntities))
                    .First();
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
    }
}
