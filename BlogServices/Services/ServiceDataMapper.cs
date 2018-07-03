using BlogDAL.Entity;
using BlogServices.Services;
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

                if (childCommentEntities == null)
                {
                    continue;
                }

                List<CommentDTO> childCommentDTOs = new List<CommentDTO>();

                foreach (CommentEntity childCommentEntity in childCommentEntities)
                {
                    CommentDTO childCommentDTO = MapCommentEntityToDTO(childCommentEntity);
                    childCommentDTOs.Add(childCommentDTO);
                }

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

        public PostEntity MapEditedPostDTOToEntity(EditedPostDTO editedPostDTO)
        {
            if (editedPostDTO == null)
            {
                return null;
            }

            PostEntity postEntity = new PostEntity()
            {
                PostCategory = MapCategoryDTOToEntity(editedPostDTO.PostCategory),
                Title = editedPostDTO.Title,
                ShortDescription = editedPostDTO.ShortDescription,
                Content = editedPostDTO.Content,
                ThumbnailImageSrc = editedPostDTO.ThumbnailImageSrc
            };

            return postEntity;
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
                PostCategory = MapCategoryEntityToDTO(categoryEntity)
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

        public CategoryEntity MapEditedCategoryDTOToEntity(EditedCategoryDTO editedCategoryDTO)
        {
            if (editedCategoryDTO == null)
            {
                return null;
            }

            CategoryEntity categoryEntity = new CategoryEntity()
            {
                CategoryId = editedCategoryDTO.CategoryId,
                Name = editedCategoryDTO.Name,
                Description = editedCategoryDTO.Description
            };

            return categoryEntity;
        }

        public CategoryEntity MapCategoryDTOToEntity(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return null;
            }

            CategoryEntity categoryEntity = new CategoryEntity()
            {
                Name = categoryDTO.Name
            };

            return categoryEntity;
        }

        public PaginationDTO<PostCardDTO> MapPostCardPaginationEntityToDTO(PaginationEntity<PostEntity> postPaginationEntity)
        {
            if (postPaginationEntity == null)
            {
                return null;
            }

            List<PostEntity> postEntities = postPaginationEntity.Entities;
            List<PostCardDTO> postCardDTOs = MapPostCardEntitiesToDTOs(postEntities);

            PaginationDTO<PostCardDTO> postCardPaginationDTO = new PaginationDTO<PostCardDTO>()
            {
                DTOs = postCardDTOs,
                HasNext = postPaginationEntity.HasNext,
                HasPrevious = postPaginationEntity.HasPrevious,
                Pages = postPaginationEntity.Pages,
            };

            return postCardPaginationDTO;
        }

        public List<PostCardDTO> MapPostCardEntitiesToDTOs(List<PostEntity> postEntities)
        {
            if (postEntities == null)
            {
                return null;
            }

            List<PostCardDTO> postCardDTOs = new List<PostCardDTO>();

            foreach (PostEntity postEntity in postEntities)
            {
                PostCardDTO postCardDTO = MapPostCardEntityToDTO(postEntity);
                postCardDTOs.Add(postCardDTO);
            }

            return postCardDTOs;
        }

        public PaginationDTO<CommentDTO> MapCommentPaginationEntityToDTO(PaginationEntity<CommentEntity> commentPaginationEntity)
        {
            if (commentPaginationEntity == null)
            {
                return null;
            }

            List<CommentEntity> commentEntities = commentPaginationEntity.Entities;
            List<CommentDTO> commentDTOs = MapCommentEntitiesToDTOs(commentEntities);

            PaginationDTO<CommentDTO> commentPaginationDTO = new PaginationDTO<CommentDTO>()
            {
                DTOs = commentDTOs,
                HasNext = commentPaginationEntity.HasNext,
                HasPrevious = commentPaginationEntity.HasPrevious,
                Pages = commentPaginationEntity.Pages
            };

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
            if (postPaginationEntities == null)
            {
                return null;
            }

            List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs = new List<PaginationDTO<PostCardDTO>>();

            foreach (PaginationEntity<PostEntity> postPaginationEntity in postPaginationEntities)
            {
                PaginationDTO<PostCardDTO> postCardPaginationDTO = MapPostCardPaginationEntityToDTO(postPaginationEntity);
                postCardPaginationDTOs.Add(postCardPaginationDTO);
            }

            return postCardPaginationDTOs;
        }
    }
}
