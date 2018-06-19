using BlogDAL.Entity;
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
        PostDTOWithPaginatedComments GetPostDTOWithPaginatedComments(string id, int pageSize);
        bool AddEditedPostDTO(EditedPostDTO editedPostDTO);
        PaginationDTO<PostCardDTO> GetPostCardPaginationDTOWithCategory(string category, int pageNumber, int pageSize);
        List<PaginationDTO<PostCardDTO>> GetPostCardPaginationDTOs(int pageSize);
    }
}
