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

        List<CommentModel> MapCommentDTOsToModels(List<CommentDTO> commentDTOs);

        PostModel MapPostDTOToModel(PostDTO postDTO);

        EditedPostDTO MapEditedPostModelToDTO(EditedPostModel editedPostModel);

        PostCardModel MapPostCardDTOToModel(PostCardDTO postCardDTO);

        CommentModel MapCommentDTOToModel(CommentDTO commentDTO);

        CategoryModel MapCategoryDTOToModel(CategoryDTO categoryDTO);

        EditedCategoryDTO MapEditedCategoryModelToDTO(EditedCategoryModel editedCategoryModel);

        CategoryDTO MapCategoryModelToDTO(CategoryModel categoryModel);

        List<PostCardModel> MapPostCardDTOsToModels(List<PostCardDTO> postCardDTOs);

        PaginationModel<PostCardModel> MapPostCardPaginationDTOToModel(PaginationDTO<PostCardDTO> postCardPaginationDTO);

        PostModelWithPaginatedComments MapPostDTOToModelWithPaginatedComments(PostDTOWithPaginatedComments postDTOWithPaginatedComments);

        List<CategoryModel> MapCategoryDTOsToModels(List<CategoryDTO> categoryDTOs);

        List<PaginationModel<PostCardModel>> MapPostCardPaginationDTOsToModels(List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs);

        PaginationModel<CommentModel> MapCommentPaginationDTOToModel(PaginationDTO<CommentDTO> commentPaginationDTO);
    }
}