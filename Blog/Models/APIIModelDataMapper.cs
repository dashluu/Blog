using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    public interface APIIModelDataMapper
    {
        //Map post
        APIPostCategoryModel MapPostCategoryDTOToModel(CategoryDTO categoryDTO);
        CategoryDTO MapPostCategoryModelToDTO(APIPostCategoryModel postCategoryModel);
        PostDTO MapEditedPostModelToDTO(APIEditedPostModel editedPostModel);
        APIEditedPostModel MapEditedPostDTOToModel(PostDTO postDTO);
        APIPostCardModel MapPostCardDTOToModel(PostCardDTO postCardDTO);
        List<APIPostCardModel> MapPostCardDTOsToModels(List<PostCardDTO> postCardDTOs);
        APIPaginationModel<APIPostCardModel> MapPostCardPaginationDTOToModel(PaginationDTO<PostCardDTO> postCardPaginationDTO);

        //Map category
        APICategoryModel MapCategoryDTOToModel(CategoryDTO categoryDTO);
        List<APICategoryModel> MapCategoryDTOsToModels(List<CategoryDTO> categoryDTOs);
        CategoryDTO MapEditedCategoryModelToDTO(APIEditedCategoryModel editedCategoryModel);

        //Map comment
        APICommentModel MapCommentDTOToModel(CommentDTO commentDTO);
        List<APICommentModel> MapCommentDTOsToModels(List<CommentDTO> commentDTOs);
        PaginationModel<APICommentModel> MapCommentPaginationDTOToModel(PaginationDTO<CommentDTO> commentPaginationDTO);

        //Map image
        APIImageModel MapImageDTOToModel(ImageDTO imageDTO);
    }
}
