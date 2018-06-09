using BlogDAL.Entity;
using BlogDAL.Repository;
using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public class CommentService : ICommentService
    {
        private IPostRepository postRepository;
        private ICommentRepository commentRepository;
        private DataMapper dataMapper;

        public CommentService()
        {
            postRepository = new PostRepository();
            commentRepository = new CommentRepository(postRepository);
            dataMapper = new DataMapper();
        }

        public string AddCommentDTO(string postId, string commentContent, string username)
        {
            CommentEntity commentEntity = new CommentEntity()
            {
                Content = commentContent,
                Username = username
            };

            string commentId = commentRepository.AddCommentEntity(postId, commentEntity);

            return commentId;
        }

        public bool AddChildCommentDTO(string postId, string commentId, string commentContent, string username)
        {
            CommentEntity childCommentEntity = new CommentEntity()
            {
                Content = commentContent,
                Username = username
            };

            bool addSuccessfully = commentRepository.AddChildCommentEntity(postId, commentId, childCommentEntity);

            return addSuccessfully;
        }
    }
}
