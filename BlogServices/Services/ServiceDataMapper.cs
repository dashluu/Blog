using BlogDAL.Entity;
using BlogServices.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class ServiceDataMapper : IServiceDataMapper
    {
        public List<CommentDTO> MapCommentEntitiesToDTOs(List<CommentEntity> commentEntities)
        {
            if (commentEntities == null)
            {
                return null;
            }

            List<CommentDTO> commentDTOs = new List<CommentDTO>();

            foreach (CommentEntity commentEntity in commentEntities)
            {
                CommentDTO commentDTO = MapCommentEntityToDTO(commentEntity);
                commentDTOs.Add(commentDTO);
                List<CommentEntity> childCommentEntities = commentEntity.ChildCommentEntities;
                Func<CommentEntity, CommentDTO> delegateMapper = MapCommentEntityToDTO;
                List<CommentDTO> childCommentDTOs = MapGenericList(childCommentEntities, delegateMapper);
                commentDTO.ChildCommentDTOs = childCommentDTOs;
            }

            return commentDTOs;
        }

        public PostDTO MapPostEntityToDTO(PostEntity postEntity)
        {
            if (postEntity == null)
            {
                return null;
            }

            List<CommentEntity> commentEntities = postEntity.CommentEntities;
            List<CommentDTO> commentDTOs = MapCommentEntitiesToDTOs(commentEntities);

            PostDTO postDTO = new PostDTO()
            {
                PostId = postEntity.PostId,
                PostCategory = MapCategoryEntityToDTO(postEntity.PostCategory),
                Title = postEntity.Title,
                CreatedDate = postEntity.CreatedDate,
                UpdatedDate = postEntity.UpdatedDate,
                ShortDescription = postEntity.ShortDescription,
                Content = postEntity.Content,
                ThumbnailImageSrc = postEntity.ThumbnailImageSrc,
                CommentDTOs = commentDTOs,
                CommentCount = postEntity.CommentCount
            };

            return postDTO;
        }

        public PostCardDTO MapPostCardEntityToDTO(PostEntity postEntity)
        {
            if (postEntity == null)
            {
                return null;
            }

            CategoryEntity categoryEntity = postEntity.PostCategory;

            PostCardDTO postCardDTO = new PostCardDTO()
            {
                PostId = postEntity.PostId,
                Title = postEntity.Title,
                CreatedDate = postEntity.CreatedDate,
                UpdatedDate = postEntity.UpdatedDate,
                ShortDescription = postEntity.ShortDescription,
                ThumbnailImageSrc = postEntity.ThumbnailImageSrc,
                PostCategory = MapCategoryEntityToDTO(categoryEntity),
                CommentCount = postEntity.CommentCount
            };

            return postCardDTO;
        }

        public CommentDTO MapCommentEntityToDTO(CommentEntity commentEntity)
        {
            if (commentEntity == null)
            {
                return null;
            }

            CommentDTO commentDTO = new CommentDTO()
            {
                CommentId = commentEntity.CommentId,
                Username = commentEntity.Username,
                Content = commentEntity.Content,
                CreatedDate = commentEntity.CreatedDate
            };

            return commentDTO;
        }

        public CategoryDTO MapCategoryEntityToDTO(CategoryEntity categoryEntity)
        {
            if (categoryEntity == null)
            {
                return null;
            }

            CategoryDTO categoryDTO = new CategoryDTO()
            {
                CategoryId = categoryEntity.CategoryId,
                Name = categoryEntity.Name,
                Description = categoryEntity.Description,
                PostCount = categoryEntity.PostCount
            };

            return categoryDTO;
        }

        public CategoryEntity MapCategoryDTOToEntity(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return null;
            }

            CategoryEntity categoryEntity = new CategoryEntity()
            {
                CategoryId = categoryDTO.CategoryId,
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                PostCount = categoryDTO.PostCount
            };

            return categoryEntity;
        }

        public PaginationDTO<PostCardDTO> MapPostCardPaginationEntityToDTO(PaginationEntity<PostEntity> postPaginationEntity)
        {
            Func<List<PostEntity>, List<PostCardDTO>> delegateMapper = MapPostCardEntitiesToDTOs;
            PaginationDTO<PostCardDTO> postCardPaginationDTO = MapGenericPagination(postPaginationEntity, delegateMapper);

            return postCardPaginationDTO;
        }

        public List<PostCardDTO> MapPostCardEntitiesToDTOs(List<PostEntity> postEntities)
        {
            Func<PostEntity, PostCardDTO> delegateMapper = MapPostCardEntityToDTO;
            List<PostCardDTO> postCardDTOs = MapGenericList(postEntities, delegateMapper);

            return postCardDTOs;
        }

        public PaginationDTO<CommentDTO> MapCommentPaginationEntityToDTO(PaginationEntity<CommentEntity> commentPaginationEntity)
        {
            Func<List<CommentEntity>, List<CommentDTO>> delegateMapper = MapCommentEntitiesToDTOs;
            PaginationDTO<CommentDTO> commentPaginationDTO = MapGenericPagination(commentPaginationEntity, delegateMapper);

            return commentPaginationDTO;
        }

        public PostDTOWithPaginatedComments MapPostEntityToDTOWithPaginatedComments(PostEntityWithPaginatedComments postEntityWithPaginatedComments)
        {
            if (postEntityWithPaginatedComments == null)
            {
                return null;
            }

            PostEntity postEntity = postEntityWithPaginatedComments.Post;
            PaginationEntity<CommentEntity> commentPaginationEntity = postEntityWithPaginatedComments.CommentPaginationEntity;

            PostDTOWithPaginatedComments postDTOWithPaginatedComments = new PostDTOWithPaginatedComments()
            {
                Post = MapPostEntityToDTO(postEntity),
                CommentPaginationDTO = MapCommentPaginationEntityToDTO(commentPaginationEntity)
            };

            return postDTOWithPaginatedComments;
        }

        public List<PaginationDTO<PostCardDTO>> MapPostCardPaginationEntitiesToDTOs(List<PaginationEntity<PostEntity>> postPaginationEntities)
        {
            Func<PaginationEntity<PostEntity>, PaginationDTO<PostCardDTO>> delegateMapper = MapPostCardPaginationEntityToDTO;
            List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs = MapGenericList(postPaginationEntities, delegateMapper);

            return postCardPaginationDTOs;
        }

        public PostEntity MapPostDTOToEntity(PostDTO postDTO)
        {
            if (postDTO == null)
            {
                return null;
            }

            PostEntity postEntity = new PostEntity()
            {
                PostId = postDTO.PostId,
                PostCategory = MapCategoryDTOToEntity(postDTO.PostCategory),
                Title = postDTO.Title,
                ShortDescription = postDTO.ShortDescription,
                Content = postDTO.Content,
                ThumbnailImageSrc = postDTO.ThumbnailImageSrc,
                CreatedDate = postDTO.CreatedDate,
                UpdatedDate = postDTO.UpdatedDate
            };

            return postEntity;
        }

        public List<CategoryDTO> MapCategoryEntitiesToDTOs(List<CategoryEntity> categoryEntities)
        {
            Func<CategoryEntity, CategoryDTO> delegateMapper = MapCategoryEntityToDTO;
            List<CategoryDTO> categoryDTOs = MapGenericList(categoryEntities, delegateMapper);

            return categoryDTOs;
        }

        public CommentEntity MapCommentDTOToEntity(CommentDTO commentDTO)
        {
            if (commentDTO == null)
            {
                return null;
            }

            CommentEntity commentEntity = new CommentEntity()
            {
                CommentId = commentDTO.CommentId,
                Username = commentDTO.Username,
                Content = commentDTO.Content,
                CreatedDate = commentDTO.CreatedDate,
                ParentCommentId = commentDTO.ParentCommentId,
                PostId = commentDTO.PostId
            };

            return commentEntity;
        }

        public UserEntity MapUserDTOToEntity(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            UserEntity userEntity = new UserEntity()
            {
                Id = userDTO.Id,
                UserName = userDTO.UserName,
                Email = userDTO.Email
            };

            return userEntity;
        }

        public UserDTO MapUserEntityToDTO(UserEntity userEntity, RoleEntity adminRoleEntity = null)
        {
            if (userEntity == null)
            {
                return null;
            }

            UserDTO userDTO = new UserDTO()
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                Email = userEntity.Email
            };

            if (adminRoleEntity == null)
            {
                userDTO.LockoutEnabled = userEntity.LockoutEnabled;
                return userDTO;
            }

            foreach(IdentityUserRole userRole in userEntity.Roles)
            {
                if (userRole.RoleId.Equals(adminRoleEntity.Id))
                {
                    userDTO.IsAdmin = true;
                }
            }

            if (userDTO.IsAdmin)
            {
                userDTO.LockoutEnabled = false;
            }
            else
            {
                userDTO.LockoutEnabled = userEntity.LockoutEnabled;
            }

            return userDTO;
        }

        public PaginationDTO<UserDTO> MapUserPaginationEntityToDTO(PaginationEntity<UserEntity> userPaginationEntity, RoleEntity adminRoleEntity = null)
        {
            if (userPaginationEntity == null)
            {
                return null;
            }

            List<UserEntity> userEntities = userPaginationEntity.Entities;
            List<UserDTO> userDTOs = MapUserEntitiesToDTOs(userEntities, adminRoleEntity);

            PaginationDTO<UserDTO> userPaginationDTO = new PaginationDTO<UserDTO>()
            {
                DTOs = userDTOs,
                HasNext = userPaginationEntity.HasNext,
                HasPrevious = userPaginationEntity.HasPrevious,
                PageNumber = userPaginationEntity.PageNumber,
                PageSize = userPaginationEntity.PageSize,
                Pages = userPaginationEntity.Pages
            };

            return userPaginationDTO;
        }

        public List<UserDTO> MapUserEntitiesToDTOs(List<UserEntity> userEntities, RoleEntity adminRoleEntity = null)
        {
            if (userEntities == null)
            {
                return null;
            }

            List<UserDTO> userDTOs = new List<UserDTO>();

            foreach (UserEntity userEntity in userEntities)
            {
                UserDTO userDTO = MapUserEntityToDTO(userEntity, adminRoleEntity);
                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }

        public List<T2> MapGenericList<T1, T2>(List<T1> list1, Func<T1, T2> delegateMapper)
        {
            if (list1 == null)
            {
                return null;
            }

            List<T2> list2 = new List<T2>();

            foreach (T1 object1 in list1)
            {
                T2 object2 = delegateMapper(object1);
                list2.Add(object2);
            }

            return list2;
        }

        public PaginationDTO<T2> MapGenericPagination<T1, T2>(PaginationEntity<T1> pagination1, Func<List<T1>, List<T2>> delegateMapper)
        {
            if (pagination1 == null)
            {
                return null;
            }

            PaginationDTO<T2> paginationDTO = new PaginationDTO<T2>
            {
                DTOs = delegateMapper(pagination1.Entities),
                HasNext = pagination1.HasNext,
                HasPrevious = pagination1.HasPrevious,
                PageNumber = pagination1.PageNumber,
                PageSize = pagination1.PageSize,
                Pages = pagination1.Pages
            };

            return paginationDTO;
        }
    }
}
