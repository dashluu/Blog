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
        (List<CommentDTO> commentDTOs, bool end) PaginateComment(string postId, int skip);
        List<CommentDTO> GetChildCommentDTOs(string commentId);
    }
}
