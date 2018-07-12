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
                PageNumber = postPaginationEntity.PageNumber,
                PageSize = postPaginationEntity.PageSize,
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
                PageNumber = commentPaginationEntity.PageNumber,
                PageSize = commentPaginationEntity.PageSize,
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
            if (categoryEntities == null)
            {
                return null;
            }

            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();

            foreach (CategoryEntity categoryEntity in categoryEntities)
            {
                CategoryDTO categoryDTO = MapCategoryEntityToDTO(categoryEntity);
                categoryDTOs.Add(categoryDTO);
            }

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
    }
}
