using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface IBlogService
    {
        List<PostCardDTO> GetPostCardDTOs();
        List<PostCardDTO> GetPostCardDTOsWithCategory(string category);
        PostDTO GetPostDTO(string id);
        string AddCommentDTO(string postId, string commentContent, string username);
        bool AddChildCommentDTO(string postId, string commentId, string commentContent, string username);

    }
}
