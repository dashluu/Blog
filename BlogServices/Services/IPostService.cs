using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface IPostService
    {
        List<PostCardDTO> GetPostCardDTOs();
        List<PostCardDTO> GetPostCardDTOsWithCategory(string category);
        (PostDTO postDTO, bool end) GetPostDTOWithCommentPagination(string id);
        bool AddEditedPostDTO(EditedPostDTO editedPostDTO);
    }
}
