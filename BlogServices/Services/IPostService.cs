using BlogDAL.Entity;
using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface IPostService
    {
        bool AddEditedPostDTO(EditedPostDTO editedPostDTO);

        PostDTOWithPaginatedComments GetPostDTOWithPaginatedComments(string id, int pageSize);
        PaginationDTO<PostCardDTO> GetPostCardPaginationDTO(string category, int pageNumber, int pageSize);
        List<PaginationDTO<PostCardDTO>> GetPostCardPaginationDTOs(int pageSize);

        PaginationDTO<PostCardDTO> RemovePostDTOWithReloadedPagination(string category, string postId, int pageNumber, int pageSize);
    }
}
