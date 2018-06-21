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
        List<CommentDTO> GetChildCommentDTOs(string commentId, int skip);
        PaginationDTO<CommentDTO> GetCommentPaginationDTOWithPost(string postId, int skip, int pageSize);
    }
}
