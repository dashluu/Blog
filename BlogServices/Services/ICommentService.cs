using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface ICommentService
    {
        string AddCommentDTO(string postId, string commentContent, string username);
        bool AddChildCommentDTO(string postId, string commentId, string commentContent, string username);
        bool RemoveCommentDTO(string commentId);
        List<CommentDTO> GetChildCommentDTOs(string commentId, int skip);
        PaginationDTO<CommentDTO> GetCommentPaginationDTOWithPost(string postId, int commentCount, int skip, int pageSize);
        PaginationDTO<CommentDTO> GetCommentPaginationDTO(int pageNumber, int pageSize);
        PaginationDTO<CommentDTO> GetChildCommentPaginationDTO(string commentId, int pageNumber, int pageSize);
        PaginationDTO<CommentDTO> SearchCommentWithPaginationDTO(string searchQuery, int pageNumber, int pageSize);
    }
}
