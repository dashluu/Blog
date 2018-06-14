using BlogDAL.Entity;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class ServiceDataMapper : IServiceDataMapper
    {
        public string ComputeUpdateTime(DateTime updateTime)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timeSpan = currentTime.Subtract(updateTime);
            int nday = timeSpan.Days;
            int nhour = timeSpan.Hours;
            int nmin = timeSpan.Minutes;
            int nsec = timeSpan.Seconds;
            string internalTime;

            if (nday > 7)
            {
                internalTime = " on " + FormatTime(updateTime);
                return internalTime;
            }

            if (nday >= 1)
            {
                internalTime = nday.ToString() + ((nday > 1) ? " days" : " day") + " ago";
                return internalTime;
            }

            if (nhour >= 1)
            {
                internalTime = nhour.ToString() + ((nhour > 1) ? " hours" : " hour") + " ago";
                return internalTime;
            }

            if (nmin >= 1)
            {
                internalTime = nmin.ToString() + " min ago";
                return internalTime;
            }

            internalTime = nsec.ToString() + " sec ago";

            return internalTime;
        }

        public string FormatTime(DateTime time)
        {
            return time.ToString("dd MMMM");
        }

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
                CreatedDate = FormatTime(postEntity.CreatedDate),
                UpdatedDate = ComputeUpdateTime(postEntity.UpdatedDate),
                ShortDescription = postEntity.ShortDescription,
                Content = postEntity.Content,
                ThumbnailImageSrc = postEntity.ThumbnailImageSrc,
                CommentDTOs = commentDTOs
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

            PostCardDTO postCardDTO = new PostCardDTO()
            {
                PostId = postEntity.PostId,
                Title = postEntity.Title,
                CreatedDate = FormatTime(postEntity.CreatedDate),
                UpdatedDate = ComputeUpdateTime(postEntity.UpdatedDate),
                ShortDescription = postEntity.ShortDescription,
                ThumbnailImageSrc = postEntity.ThumbnailImageSrc
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
                Name = categoryEntity.Name
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
    }
}
