using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface ICommentService
    {
        CommentDTO AddComment(string postId, string commentContent, string username);
        CommentDTO AddChildComment(string postId, string commentId, string commentContent, string username);

        PaginationDTO<CommentDTO> RemoveCommentDTOWithReloadedPagination(string postId, string commentId, int pageNumber, int pageSize);

        List<CommentDTO> GetChildCommentDTOs(string commentId, int skip);
        PaginationDTO<CommentDTO> GetChildCommentPaginationDTO(string commentId, int pageNumber, int pageSize);

        PaginationDTO<CommentDTO> GetCommentPaginationDTOOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize);
        PaginationDTO<CommentDTO> GetCommentPaginationDTO(string postId, int pageNumber, int pageSize);

        PaginationDTO<CommentDTO> SearchCommentWithPaginationDTO(string postId, string searchQuery, int pageNumber, int pageSize);
    }
}
