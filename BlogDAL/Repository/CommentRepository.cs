using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace BlogDAL.Repository
{
    public class CommentRepository : BaseRepository<CommentEntity, BlogDBContext>, ICommentRepository
    {
        private IPostRepository postRepository;
        private Pagination pagination;

        public CommentRepository(IPostRepository postRepository, Pagination pagination)
        {
            this.postRepository = postRepository;
            this.pagination = pagination;
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public string AddCommentEntity(string postId, CommentEntity commentEntity)
        {
            try
            {
                string commentId = GenerateId();
                commentEntity.CommentId = commentId;
                commentEntity.CreatedDate = DateTime.Now;
                commentEntity.Post = postRepository.GetPostEntity(postId);
                Add(commentEntity);
                Context.SaveChanges();

                return commentId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CommentEntity GetCommentEntity(string postId, string commentId)
        {
            try
            {
                CommentEntity commentEntity = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(postId) && x.CommentId.Equals(commentId))
                    .First();

                return commentEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AddChildCommentEntity(string postId, string commentId, CommentEntity childCommentEntity)
        {
            try
            {
                string childCommentId = GenerateId();
                childCommentEntity.CommentId = childCommentId;
                childCommentEntity.CreatedDate = DateTime.Now;
                childCommentEntity.RootComment = GetCommentEntity(postId, commentId);
                Add(childCommentEntity);
                Context.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public (List<CommentEntity> commentEntities, bool end) Paginate(string postId, int skip)
        {
            try
            {
                IQueryable<CommentEntity> query = Context.CommentEntities
                    .Where(x => x.Post.PostId.Equals(postId));

                int commentPageSize = pagination.CommentPageSize;

                List<CommentEntity> commentEntities = query
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip(() => skip)
                    .Take(() => commentPageSize)
                    .ToList();

                int countComment = query.Count();
                bool end = (skip + commentPageSize) >= countComment;

                return (commentEntities, end);
            }
            catch (Exception)
            {
                return (null, false);
            }
        }

        public List<CommentEntity> GetChildCommentEntities(string commentId)
        {
            try
            {
                List<CommentEntity> commentEntities = Context.CommentEntities
                    .Where(x => x.RootComment.CommentId.Equals(commentId))
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();

                return commentEntities;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
