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
        PaginationDTO<PostCardDTO> GetPostCardPaginationDTO(string category, int pageNumber, int pageSize);
        List<PaginationDTO<PostCardDTO>> GetPostCardPaginationDTOs(int pageSize);
        PostDTO GetPostDTO(string id);

        PaginationDTO<PostCardDTO> RemovePostDTOWithReloadedPagination(string category, string postId, int pageNumber, int pageSize);
    }
}
