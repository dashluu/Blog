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
        private IServiceDataMapper dataMapper;

        public CommentService(IPostRepository postRepository, ICommentRepository commentRepository, IServiceDataMapper dataMapper)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.dataMapper = dataMapper;
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
