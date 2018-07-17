using BlogDAL.Entity;
using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface IPostService
    {
        bool AddPost(PostDTO postDTO);

        bool UpdatePost(PostDTO postDTO);

        PostDTOWithPaginatedComments GetPostDTOWithPaginatedComments(string id, int pageSize);
        PaginationDTO<PostCardDTO> GetPostCardPaginationDTO(int pageNumber, int pageSize, string category = null, string searchQuery = null);
        List<PaginationDTO<PostCardDTO>> GetPostCardPaginationDTOs(int pageSize, string searchQuery = null);
        PostDTO GetPostDTO(string id);

        bool RemovePost(string postId);
    }
}
