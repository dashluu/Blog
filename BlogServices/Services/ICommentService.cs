using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface ICommentService
    {
        bool Add(CommentDTO commentDTO);

        bool Remove(string commentId);

        List<CommentDTO> GetChildComments(string commentId);
        PaginationDTO<CommentDTO> GetCommentPaginationOfPostWithPreservedFetch(string postId, DateTime createdDate, int pageSize);
        PaginationDTO<CommentDTO> GetCommentPagination(int pageNumber, int pageSize, string postId = null, string commentId = null, string userName = null, string searchQuery = null);
    }
}
