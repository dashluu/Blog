using BlogDAL.Entity;
using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface IServiceDataMapper
    {
        List<CommentDTO> MapCommentEntitiesToDTOs(List<CommentEntity> commentEntities);

        PostDTO MapPostEntityToDTO(PostEntity postEntity);

        PostEntity MapEditedPostDTOToEntity(EditedPostDTO editedPostDTO);

        PostCardDTO MapPostCardEntityToDTO(PostEntity postEntity);

        List<PostCardDTO> MapPostCardEntitiesToDTOs(List<PostEntity> postEntities);

        CommentDTO MapCommentEntityToDTO(CommentEntity commentEntity);

        CategoryDTO MapCategoryEntityToDTO(CategoryEntity categoryEntity);

        CategoryEntity MapEditedCategoryDTOToEntity(EditedCategoryDTO editedCategoryDTO);

        CategoryEntity MapCategoryDTOToEntity(CategoryDTO categoryDTO);

        PaginationDTO<PostCardDTO> MapPostCardPaginationEntityToDTO(PaginationEntity<PostEntity> postPaginationEntity);

        PaginationDTO<CommentDTO> MapCommentPaginationEntityToDTO(PaginationEntity<CommentEntity> commentPaginationEntity);

        PostDTOWithPaginatedComments MapPostEntityToDTOWithPaginatedComments(PostEntityWithPaginatedComments postEntityWithPaginatedComments);

        List<PaginationDTO<PostCardDTO>> MapPostCardPaginationEntitiesToDTOs(List<PaginationEntity<PostEntity>> postPaginationEntities);
    }
}
