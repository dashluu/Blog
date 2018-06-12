﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogServices.DTO;

namespace Blog.Models
{
    public class ModelDataMapper : IModelDataMapper
    {
        public List<CommentModel> MapCommentDTOsToModels(List<CommentDTO> commentDTOs)
        {
            if (commentDTOs == null)
            {
                return null;
            }

            List<CommentModel> commentModels = new List<CommentModel>();

            foreach (CommentDTO commentDTO in commentDTOs)
            {
                CommentModel commentModel = MapCommentDTOToModel(commentDTO);
                commentModels.Add(commentModel);
                List<CommentDTO> childCommentDTOs = commentDTO.ChildCommentDTOs;

                if (childCommentDTOs == null)
                {
                    continue;
                }

                List<CommentModel> childCommentModels = new List<CommentModel>();

                foreach (CommentDTO childCommentDTO in childCommentDTOs)
                {
                    CommentModel childCommentModel = MapCommentDTOToModel(childCommentDTO);
                    childCommentModels.Add(childCommentModel);
                }

                commentModel.ChildCommentModels = childCommentModels;
            }

            return commentModels;
        }

        public CommentModel MapCommentDTOToModel(CommentDTO commentDTO)
        {
            if (commentDTO == null)
            {
                return null;
            }

            CommentModel commentModel = new CommentModel()
            {
                CommentId = commentDTO.CommentId,
                Username = commentDTO.Username,
                Content = commentDTO.Content
            };

            return commentModel;
        }

        public EditedPostDTO MapEditedPostModelToDTO(EditedPostModel editedPostModel)
        {
            if (editedPostModel == null)
            {
                return null;
            }

            EditedPostDTO editedPostDTO = new EditedPostDTO()
            {
                Title = editedPostModel.Title,
                PostCategory = editedPostModel.PostCategory,
                ShortDescription = editedPostModel.ShortDescription,
                ThumbnailImageSrc = editedPostModel.ThumbnailImageSrc
            };

            return editedPostDTO;
        }

        public PostCardModel MapPostCardDTOToModel(PostCardDTO postCardDTO)
        {
            if (postCardDTO == null)
            {
                return null;
            }

            PostCardModel postCardModel = new PostCardModel()
            {
                PostId = postCardDTO.PostId,
                Title = postCardDTO.Title,
                ShortDescription = postCardDTO.ShortDescription,
                ThumbnailImageSrc = postCardDTO.ThumbnailImageSrc,
                CreatedDate = postCardDTO.CreatedDate,
                UpdatedDate = postCardDTO.UpdatedDate
            };

            return postCardModel;
        }

        public PostModel MapPostDTOToModel(PostDTO postDTO)
        {
            if (postDTO == null)
            {
                return null;
            }

            List<CommentDTO> commentDTOs = postDTO.CommentDTOs;
            List<CommentModel> commentModels = MapCommentDTOsToModels(commentDTOs);

            PostModel postModel = new PostModel()
            {
                PostId = postDTO.PostId,
                Title = postDTO.Title,
                PostCategory = postDTO.PostCategory,
                ShortDescription = postDTO.ShortDescription,
                ThumbnailImageSrc = postDTO.ThumbnailImageSrc,
                Content = postDTO.Content,
                CreatedDate = postDTO.CreatedDate,
                UpdatedDate = postDTO.UpdatedDate,
                CommentModels = commentModels
            };

            return postModel;
        }
    }
}