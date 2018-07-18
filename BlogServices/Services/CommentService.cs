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

        public bool Add(CommentDTO commentDTO)
        {
            commentDTO.CommentId = hashService.GenerateId();
            commentDTO.CreatedDate = DateTime.Now;
            commentDTO.Username = "username";

            CommentEntity commentEntity = dataMapper.MapCommentDTOToEntity(commentDTO);
            bool addSuccessfully = commentRepository.Add(commentEntity);

            return addSuccessfully;
        }

        public List<CommentDTO> GetChildComments(string commentId)
        {
            List<CommentEntity> childCommentEntities = commentRepository.GetChildComments(commentId);
            List<CommentDTO> childCommentDTOs = dataMapper.MapCommentEntitiesToDTOs(childCommentEntities);

            return childCommentDTOs;
        }

        public PaginationDTO<CommentDTO> GetCommentPaginationOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPaginationOfPostWithPreservedFetch(postId, createdDate, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> GetCommentPagination(int pageNumber, int pageSize, string postId = null, string commentId = null, string searchQuery = null)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPagination(pageNumber, pageSize, postId, commentId, searchQuery);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public bool Remove(string commentId)
        {
            bool removeSuccessfully = commentRepository.Remove(commentId);
            return removeSuccessfully;
        }
    }
}
