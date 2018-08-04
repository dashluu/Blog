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
            Func<PostCardDTO, APIPostCardModel> delegateMapper = MapPostCardDTOToModel;
            List<APIPostCardModel> postCardModels = MapGenericList(postCardDTOs, delegateMapper);

            return postCardModels;
        }

        public APIPaginationModel<APIPostCardModel> MapPostCardPaginationDTOToModel(PaginationDTO<PostCardDTO> postCardPaginationDTO)
        {
            Func<List<PostCardDTO>, List<APIPostCardModel>> delegateMapper = MapPostCardDTOsToModels;
            APIPaginationModel<APIPostCardModel> postCardPaginationModel = MapGenericPagination(postCardPaginationDTO, delegateMapper);

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
            Func<CategoryDTO, APICategoryModel> delegateMapper = MapCategoryDTOToModel;
            List<APICategoryModel> categoryModels = MapGenericList(categoryDTOs, delegateMapper);

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
                Func<CommentDTO, APICommentModel> delegateMapper = MapCommentDTOToModel;
                List<APICommentModel> childCommentModels = MapGenericList(childCommentDTOs, delegateMapper);
                commentModel.ChildCommentModels = childCommentModels;
            }

            return commentModels;
        }

        public APIPaginationModel<APICommentModel> MapCommentPaginationDTOToModel(PaginationDTO<CommentDTO> commentPaginationDTO)
        {
            Func<List<CommentDTO>, List<APICommentModel>> delegateMapper = MapCommentDTOsToModels;
            APIPaginationModel<APICommentModel> commentPaginationModel = MapGenericPagination(commentPaginationDTO, delegateMapper);

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
                LockoutEnabled = userDTO.LockoutEnabled,
                IsAdmin = userDTO.IsAdmin
            };

            return userModel;
        }

        public List<APIUserModel> MapUserDTOsToModels(List<UserDTO> userDTOs)
        {
            Func<UserDTO, APIUserModel> delegateMapper = MapUserDTOToModel;
            List<APIUserModel> userModels = MapGenericList(userDTOs, delegateMapper);

            return userModels;
        }

        public APIPaginationModel<APIUserModel> MapUserPaginationDTOToModel(PaginationDTO<UserDTO> userPaginationDTO)
        {
            Func<List<UserDTO>, List<APIUserModel>> delegateMapper = MapUserDTOsToModels;
            APIPaginationModel<APIUserModel> userPaginationModel = MapGenericPagination(userPaginationDTO, delegateMapper);

            return userPaginationModel;
        }

        public UserDTO MapUserLoginModelToDTO(APIUserLoginModel userModel)
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

        public APIPaginationModel<T2> MapGenericPagination<T1, T2>(PaginationDTO<T1> pagination1, Func<List<T1>, List<T2>> delegateMapper)
        {
            if (pagination1 == null)
            {
                return null;
            }

            APIPaginationModel<T2> paginationModel = new APIPaginationModel<T2>
            {
                Models = delegateMapper(pagination1.DTOs),
                HasNext = pagination1.HasNext,
                HasPrevious = pagination1.HasPrevious,
                PageNumber = pagination1.PageNumber,
                PageSize = pagination1.PageSize,
                Pages = pagination1.Pages
            };

            return paginationModel;
        }
    }
}