using BlogDAL.Entity;
using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface IPostService
    {
        bool Add(PostDTO postDTO);

        bool Update(PostDTO postDTO);

        PostDTOWithPaginatedComments GetPostWithPaginatedComments(string id, int pageSize);
        PaginationDTO<PostCardDTO> GetPostCardPagination(int pageNumber, int pageSize, string category = null, string searchQuery = null);
        List<PaginationDTO<PostCardDTO>> GetPostCardPaginationList(int pageSize, string searchQuery = null);
        PostDTO GetPost(string id);

        bool Remove(string postId);
    }
}
