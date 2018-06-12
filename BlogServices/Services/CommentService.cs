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

        public (List<CommentDTO> commentDTOs, bool end) PaginateComment(string postId, int skip)
        {
            (List<CommentEntity> commentEntities, bool end) pagination = commentRepository.Paginate(postId, skip);
            List<CommentEntity> commentEntities = pagination.commentEntities;
            List<CommentDTO> commentDTOs = dataMapper.MapCommentEntitiesToDTOs(commentEntities);

            return (commentDTOs, pagination.end);
        }

        public List<CommentDTO> GetChildCommentDTOs(string commentId)
        {
            List<CommentEntity> childCommentEntities = commentRepository.GetChildCommentEntities(commentId);
            List<CommentDTO> childCommentDTOs = dataMapper.MapCommentEntitiesToDTOs(childCommentEntities);

            return childCommentDTOs;
        }
    }
}
