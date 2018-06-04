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
        PostDTO GetPost(string id);
    }
}
