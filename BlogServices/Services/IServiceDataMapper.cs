﻿using BlogDAL.Entity;
using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface IServiceDataMapper
    {
        //Map post
        PostEntity MapPostDTOToEntity(PostDTO postDTO);
        PostDTO MapPostEntityToDTO(PostEntity postEntity);
        PostCardDTO MapPostCardEntityToDTO(PostEntity postEntity);
        List<PostCardDTO> MapPostCardEntitiesToDTOs(List<PostEntity> postEntities);
        PostDTOWithPaginatedComments MapPostEntityToDTOWithPaginatedComments(PostEntityWithPaginatedComments postEntityWithPaginatedComments);
        List<PaginationDTO<PostCardDTO>> MapPostCardPaginationEntitiesToDTOs(List<PaginationEntity<PostEntity>> postPaginationEntities);
        PaginationDTO<PostCardDTO> MapPostCardPaginationEntityToDTO(PaginationEntity<PostEntity> postPaginationEntity);

        //Map comment
        PaginationDTO<CommentDTO> MapCommentPaginationEntityToDTO(PaginationEntity<CommentEntity> commentPaginationEntity);
        CommentDTO MapCommentEntityToDTO(CommentEntity commentEntity);
        List<CommentDTO> MapCommentEntitiesToDTOs(List<CommentEntity> commentEntities);
        CommentEntity MapCommentDTOToEntity(CommentDTO commentDTO);

        //Map category
        CategoryDTO MapCategoryEntityToDTO(CategoryEntity categoryEntity);
        CategoryEntity MapCategoryDTOToEntity(CategoryDTO categoryDTO);
        List<CategoryDTO> MapCategoryEntitiesToDTOs(List<CategoryEntity> categoryEntities);

        //Map user
        UserEntity MapUserDTOToEntity(UserDTO userDTO);
        UserDTO MapUserEntityToDTO(UserEntity userEntity, RoleEntity adminRoleEntity = null);
        PaginationDTO<UserDTO> MapUserPaginationEntityToDTO(PaginationEntity<UserEntity> userPaginationEntity, RoleEntity adminRoleEntity = null);
        List<UserDTO> MapUserEntitiesToDTOs(List<UserEntity> userEntities, RoleEntity adminRoleEntity = null);

        List<T2> MapGenericList<T1, T2>(List<T1> list1, Func<T1, T2> delegateMapper);
        PaginationDTO<T2> MapGenericPagination<T1, T2>(PaginationEntity<T1> pagination1, Func<List<T1>, List<T2>> delegateMapper);
    }
}
