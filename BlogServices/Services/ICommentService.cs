using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface ICommentService
    {
        bool AddComment(CommentDTO commentDTO);

        PaginationDTO<CommentDTO> RemoveCommentDTOWithReloadedPagination(string commentId, int pageNumber, int pageSize, string postId = null);

        List<CommentDTO> GetChildCommentDTOs(string commentId, int skip);
        PaginationDTO<CommentDTO> GetChildCommentPaginationDTO(string commentId, int pageNumber, int pageSize);

        PaginationDTO<CommentDTO> GetCommentPaginationDTOOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize);
        PaginationDTO<CommentDTO> GetCommentPaginationDTO(int pageNumber, int pageSize, string postId = null);

        PaginationDTO<CommentDTO> SearchCommentWithPaginationDTO(string searchQuery, int pageNumber, int pageSize, string postId = null);
    }
}
