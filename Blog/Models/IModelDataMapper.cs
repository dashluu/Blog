using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public interface IModelDataMapper
    {
        DateTime ParseCommentTime(string timeString);

        //Map comment
        List<CommentModel> MapCommentDTOsToModels(List<CommentDTO> commentDTOs);
        CommentModel MapCommentDTOToModel(CommentDTO commentDTO);
        PaginationModel<CommentModel> MapCommentPaginationDTOToModel(PaginationDTO<CommentDTO> commentPaginationDTO);

        //Map post
        PostModel MapPostDTOToModel(PostDTO postDTO);
        PostCardModel MapPostCardDTOToModel(PostCardDTO postCardDTO);
        List<PostCardModel> MapPostCardDTOsToModels(List<PostCardDTO> postCardDTOs);
        PaginationModel<PostCardModel> MapPostCardPaginationDTOToModel(PaginationDTO<PostCardDTO> postCardPaginationDTO);
        PostModelWithPaginatedComments MapPostDTOToModelWithPaginatedComments(PostDTOWithPaginatedComments postDTOWithPaginatedComments);
        List<PaginationModel<PostCardModel>> MapPostCardPaginationDTOsToModels(List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs);

        //Map category
        CategoryModel MapCategoryDTOToModel(CategoryDTO categoryDTO);
        CategoryDTO MapCategoryModelToDTO(CategoryModel categoryModel);
        List<CategoryModel> MapCategoryDTOsToModels(List<CategoryDTO> categoryDTOs);
    }
}