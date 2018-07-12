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
        private IHashService hashService;

        public CommentService(ICommentRepository commentRepository, IServiceDataMapper dataMapper, IHashService hashService)
        {
            this.commentRepository = commentRepository;
            this.dataMapper = dataMapper;
            this.hashService = hashService;
        }

        public bool AddComment(CommentDTO commentDTO)
        {
            commentDTO.CommentId = hashService.GenerateId();
            commentDTO.CreatedDate = DateTime.Now;
            commentDTO.Username = "username";

            CommentEntity commentEntity = dataMapper.MapCommentDTOToEntity(commentDTO);
            bool addSuccessfully = commentRepository.Add(commentEntity);

            return addSuccessfully;
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

        public PaginationDTO<CommentDTO> GetCommentPaginationDTO(int pageNumber, int pageSize, string postId = null)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPaginationEntity(pageNumber, pageSize, postId);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> GetChildCommentPaginationDTO(string commentId, int pageNumber, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetChildCommentPaginationEntity(commentId, pageNumber, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> SearchCommentWithPaginationDTO(string searchQuery, int pageNumber, int pageSize, string postId = null)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.SearchCommentWithPaginationEntity(searchQuery, pageNumber, pageSize, postId);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> RemoveCommentDTOWithReloadedPagination(string commentId, int pageNumber, int pageSize, string postId = null)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.RemoveCommentEntityWithReloadedPagination(commentId, pageNumber, pageSize, postId);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }
    }
}
