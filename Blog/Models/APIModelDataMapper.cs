using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogServices.DTO;

namespace Blog.Models
{
    public class APIModelDataMapper : APIIModelDataMapper
    {
        public APIEditedPostModel MapEditedPostDTOToModel(PostDTO postDTO)
        {
            if(postDTO == null)
            {
                return null;
            }

            APIEditedPostModel editedPostModel = new APIEditedPostModel ()
            {
                Title = postDTO.Title,
                PostCategory = MapPostCategoryDTOToModel(postDTO.PostCategory),
                ThumbnailImageSrc = postDTO.ThumbnailImageSrc,
                ShortDescription = postDTO.ShortDescription,
                Content = postDTO.Content
            };

            return editedPostModel;
        }

        public PostDTO MapEditedPostModelToDTO(APIEditedPostModel editedPostModel)
        {
            if (editedPostModel == null)
            {
                return null;
            }

            PostDTO postDTO = new PostDTO()
            {
                Title = editedPostModel.Title,
                PostCategory = MapPostCategoryModelToDTO(editedPostModel.PostCategory),
                ThumbnailImageSrc = editedPostModel.ThumbnailImageSrc,
                ShortDescription = editedPostModel.ShortDescription,
                Content = editedPostModel.Content
            };

            return postDTO;
        }

        public string FormatTime(DateTime time)
        {
            string timeString = time.ToString("MM/dd/yyyy-HH:mm");
            return timeString;
        }

        public APIPostCardModel MapPostCardDTOToModel(PostCardDTO postCardDTO)
        {
            if (postCardDTO == null)
            {
                return null;
            }

            CategoryDTO categoryDTO = postCardDTO.PostCategory;

            APIPostCardModel postCardModel = new APIPostCardModel()
            {
                PostId = postCardDTO.PostId,
                Title = postCardDTO.Title,
                ShortDescription = postCardDTO.ShortDescription,
                ThumbnailImageSrc = postCardDTO.ThumbnailImageSrc,
                CreatedDate = FormatTime(postCardDTO.CreatedDate),
                UpdatedDate = FormatTime(postCardDTO.UpdatedDate),
                PostCategory = MapPostCategoryDTOToModel(postCardDTO.PostCategory),
                CommentCount = postCardDTO.CommentCount
            };

            return postCardModel;
        }

        public APIPostCategoryModel MapPostCategoryDTOToModel(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return null;
            }

            APIPostCategoryModel postCategoryModel = new APIPostCategoryModel()
            {
                CategoryId = categoryDTO.CategoryId,
                Name = categoryDTO.Name,
                PostCount = categoryDTO.PostCount
            };

            return postCategoryModel;
        }

        public CategoryDTO MapPostCategoryModelToDTO(APIPostCategoryModel postCategoryModel)
        {
            if (postCategoryModel == null)
            {
                return null;
            }

            CategoryDTO categoryDTO = new CategoryDTO()
            {
                CategoryId = postCategoryModel.CategoryId,
                Name = postCategoryModel.Name,
                PostCount = postCategoryModel.PostCount
            };

            return categoryDTO;
        }

        public List<APIPostCardModel> MapPostCardDTOsToModels(List<PostCardDTO> postCardDTOs)
        {
            if (postCardDTOs == null)
            {
                return null;
            }

            List<APIPostCardModel> postCardModels = new List<APIPostCardModel>();

            foreach (PostCardDTO postCardDTO in postCardDTOs)
            {
                APIPostCardModel postCardModel = MapPostCardDTOToModel(postCardDTO);
                postCardModels.Add(postCardModel);
            }

            return postCardModels;
        }

        public APIPaginationModel<APIPostCardModel> MapPostCardPaginationDTOToModel(PaginationDTO<PostCardDTO> postCardPaginationDTO)
        {
            if (postCardPaginationDTO == null)
            {
                return null;
            }

            APIPaginationModel<APIPostCardModel> postCardPaginationModel = new APIPaginationModel<APIPostCardModel>
            {
                Models = MapPostCardDTOsToModels(postCardPaginationDTO.DTOs),
                HasNext = postCardPaginationDTO.HasNext,
                HasPrevious = postCardPaginationDTO.HasPrevious,
                PageNumber = postCardPaginationDTO.PageNumber,
                PageSize = postCardPaginationDTO.PageSize,
                Pages = postCardPaginationDTO.Pages
            };

            return postCardPaginationModel;
        }

        public APICategoryModel MapCategoryDTOToModel(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return null;
            }

            APICategoryModel categoryModel = new APICategoryModel()
            {
                CategoryId = categoryDTO.CategoryId,
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                PostCount = categoryDTO.PostCount
            };

            return categoryModel;
        }

        public List<APICategoryModel> MapCategoryDTOsToModels(List<CategoryDTO> categoryDTOs)
        {
            if (categoryDTOs == null)
            {
                return null;
            }

            List<APICategoryModel> categoryModels = new List<APICategoryModel>();

            foreach (CategoryDTO categoryDTO in categoryDTOs)
            {
                APICategoryModel categoryModel = MapCategoryDTOToModel(categoryDTO);
                categoryModels.Add(categoryModel);
            }

            return categoryModels;
        }

        public CategoryDTO MapEditedCategoryModelToDTO(APIEditedCategoryModel editedCategoryModel)
        {
            if (editedCategoryModel == null)
            {
                return null;
            }

            CategoryDTO categoryDTO = new CategoryDTO()
            {
                Name = editedCategoryModel.Name,
                Description = editedCategoryModel.Description
            };

            return categoryDTO;
        }

        public APICommentModel MapCommentDTOToModel(CommentDTO commentDTO)
        {
            if (commentDTO == null)
            {
                return null;
            }

            APICommentModel commentModel = new APICommentModel()
            {
                CommentId = commentDTO.CommentId,
                Username = commentDTO.Username,
                Content = commentDTO.Content,
                CreatedDate = FormatTime(commentDTO.CreatedDate)
            };

            return commentModel;
        }

        public List<APICommentModel> MapCommentDTOsToModels(List<CommentDTO> commentDTOs)
        {
            if (commentDTOs == null)
            {
                return null;
            }

            List<APICommentModel> commentModels = new List<APICommentModel>();

            foreach (CommentDTO commentDTO in commentDTOs)
            {
                APICommentModel commentModel = MapCommentDTOToModel(commentDTO);
                commentModels.Add(commentModel);
                List<CommentDTO> childCommentDTOs = commentDTO.ChildCommentDTOs;

                if (childCommentDTOs == null)
                {
                    continue;
                }

                List<APICommentModel> childCommentModels = new List<APICommentModel>();

                foreach (CommentDTO childCommentDTO in childCommentDTOs)
                {
                    APICommentModel childAPICommentModel = MapCommentDTOToModel(childCommentDTO);
                    childCommentModels.Add(childAPICommentModel);
                }

                commentModel.ChildCommentModels = childCommentModels;
            }

            return commentModels;
        }

        public APIPaginationModel<APICommentModel> MapCommentPaginationDTOToModel(PaginationDTO<CommentDTO> commentPaginationDTO)
        {
            if (commentPaginationDTO == null)
            {
                return null;
            }

            List<CommentDTO> commentDTOs = commentPaginationDTO.DTOs;
            List<APICommentModel> commentModels = MapCommentDTOsToModels(commentDTOs);

            APIPaginationModel<APICommentModel> commentPaginationModel = new APIPaginationModel<APICommentModel>()
            {
                Models = commentModels,
                HasNext = commentPaginationDTO.HasNext,
                HasPrevious = commentPaginationDTO.HasPrevious,
                PageNumber = commentPaginationDTO.PageNumber,
                PageSize = commentPaginationDTO.PageSize,
                Pages = commentPaginationDTO.Pages
            };

            return commentPaginationModel;
        }

        public APIImageModel MapImageDTOToModel(ImageDTO imageDTO)
        {
            if (imageDTO == null)
            {
                return null;
            }

            APIImageModel imageModel = new APIImageModel()
            {
                ImageId = imageDTO.ImageId,
                Extension = imageDTO.Extension
            };

            return imageModel;
        }

        public APIUserModel MapUserDTOToModel(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            APIUserModel userModel = new APIUserModel()
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
            };

            return userModel;
        }

        public List<APIUserModel> MapUserDTOsToModels(List<UserDTO> userDTOs)
        {
            if (userDTOs == null)
            {
                return null;
            }

            List<APIUserModel> userModels = new List<APIUserModel>();

            foreach (UserDTO userDTO in userDTOs)
            {
                APIUserModel userModel = MapUserDTOToModel(userDTO);
                userModels.Add(userModel);
            }

            return userModels;
        }

        public APIPaginationModel<APIUserModel> MapUserPaginationDTOToModel(PaginationDTO<UserDTO> userPaginationDTO)
        {
            if (userPaginationDTO == null)
            {
                return null;
            }

            APIPaginationModel<APIUserModel> userPaginationModel = new APIPaginationModel<APIUserModel>
            {
                Models = MapUserDTOsToModels(userPaginationDTO.DTOs),
                HasNext = userPaginationDTO.HasNext,
                HasPrevious = userPaginationDTO.HasPrevious,
                PageNumber = userPaginationDTO.PageNumber,
                PageSize = userPaginationDTO.PageSize,
                Pages = userPaginationDTO.Pages
            };

            return userPaginationModel;
        }

        public UserDTO MapUserLogInModelToDTO(APIUserLogInModel userModel)
        {
            if (userModel == null)
            {
                return null;
            }

            UserDTO userDTO = new UserDTO()
            {
                UserName = userModel.UserName,
                Password = userModel.Password
            };

            return userDTO;
        }
    }
}