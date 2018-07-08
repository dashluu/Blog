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
        private ICommentRepository commentRepository;
        private IServiceDataMapper dataMapper;

        public CommentService(ICommentRepository commentRepository, IServiceDataMapper dataMapper)
        {
            this.commentRepository = commentRepository;
            this.dataMapper = dataMapper;
        }

        public CommentDTO AddCommentDTO(string postId, string commentContent, string username)
        {
            CommentEntity commentEntity = new CommentEntity()
            {
                Content = commentContent,
                Username = username
            };

            commentEntity = commentRepository.AddCommentEntity(postId, commentEntity);
            CommentDTO commentDTO = dataMapper.MapCommentEntityToDTO(commentEntity);

            return commentDTO;
        }

        public CommentDTO AddChildCommentDTO(string postId, string commentId, string commentContent, string username)
        {
            CommentEntity childCommentEntity = new CommentEntity()
            {
                Content = commentContent,
                Username = username
            };

            childCommentEntity = commentRepository.AddChildCommentEntity(postId, commentId, childCommentEntity);
            CommentDTO childCommentDTO = dataMapper.MapCommentEntityToDTO(childCommentEntity);

            return childCommentDTO;
        }

        public List<CommentDTO> GetChildCommentDTOs(string commentId, int skip)
        {
            List<CommentEntity> childCommentEntities = commentRepository.GetChildCommentEntities(commentId, skip);
            List<CommentDTO> childCommentDTOs = dataMapper.MapCommentEntitiesToDTOs(childCommentEntities);

            return childCommentDTOs;
        }

        public PaginationDTO<CommentDTO> GetCommentPaginationDTOOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPaginationEntityOfPostWithPreservedFetch(postId, createdDate, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> GetCommentPaginationDTO(string postId, int pageNumber, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPaginationEntity(postId, pageNumber, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> GetChildCommentPaginationDTO(string commentId, int pageNumber, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetChildCommentPaginationEntity(commentId, pageNumber, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> SearchCommentWithPaginationDTO(string postId, string searchQuery, int pageNumber, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.SearchCommentWithPaginationEntity(postId, searchQuery, pageNumber, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> RemoveCommentDTOWithReloadedPagination(string postId, string commentId, int pageNumber, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.RemoveCommentEntityWithReloadedPagination(postId, commentId, pageNumber, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }
    }
}
