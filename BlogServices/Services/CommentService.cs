﻿using BlogDAL.Entity;
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

        public List<CommentDTO> GetChildCommentDTOs(string commentId, int skip)
        {
            List<CommentEntity> childCommentEntities = commentRepository.GetChildCommentEntities(commentId, skip);
            List<CommentDTO> childCommentDTOs = dataMapper.MapCommentEntitiesToDTOs(childCommentEntities);

            return childCommentDTOs;
        }

        public PaginationDTO<CommentDTO> GetCommentPaginationDTOWithPost(string postId, int commentCount, int skip, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPaginationEntityWithPost(postId, commentCount, skip, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> GetCommentPaginationDTO(int pageNumber, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetCommentPaginationEntity(pageNumber, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> GetChildCommentPaginationDTO(string commentId, int pageNumber, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.GetChildCommentPaginationEntity(commentId, pageNumber, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public PaginationDTO<CommentDTO> SearchCommentWithPaginationDTO(string searchQuery, int pageNumber, int pageSize)
        {
            PaginationEntity<CommentEntity> commentPaginationEntity = commentRepository.SearchCommentWithPaginationEntity(searchQuery, pageNumber, pageSize);
            PaginationDTO<CommentDTO> commentPaginationDTO = dataMapper.MapCommentPaginationEntityToDTO(commentPaginationEntity);

            return commentPaginationDTO;
        }

        public bool RemoveCommentDTO(string commentId)
        {
            CommentEntity commentEntity = new CommentEntity()
            {
                CommentId = commentId
            };

            bool removeSuccessfully = commentRepository.Remove(commentEntity);

            return removeSuccessfully;
        }
    }
}
