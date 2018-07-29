using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogServices.DTO;

namespace Blog.Models
{
    public class ModelDataMapper : IModelDataMapper
    {
        private string ComputeUpdateTime(DateTime updateTime)
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
                internalTime = " on " + FormatPostDate(updateTime);
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

        private string FormatPostDate(DateTime date)
        {
            return date.ToString("dd MMMM");
        }

        public List<CategoryModel> MapCategoryDTOsToModels(List<CategoryDTO> categoryDTOs)
        {
            if (categoryDTOs == null)
            {
                return null;
            }

            List<CategoryModel> categoryModels = new List<CategoryModel>();

            foreach (CategoryDTO categoryDTO in categoryDTOs)
            {
                CategoryModel categoryModel = MapCategoryDTOToModel(categoryDTO);
                categoryModels.Add(categoryModel);
            }

            return categoryModels;
        }

        public CategoryModel MapCategoryDTOToModel(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return null;
            }

            CategoryModel categoryModel = new CategoryModel()
            {
                Name = categoryDTO.Name
            };

            return categoryModel;
        }

        public CategoryDTO MapCategoryModelToDTO(CategoryModel categoryModel)
        {
            if (categoryModel == null)
            {
                return null;
            }

            CategoryDTO categoryDTO = new CategoryDTO()
            {
                Name = categoryModel.Name
            };

            return categoryDTO;
        }

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

        private string FormatCommentTime(DateTime time)
        {
            string timeString = time.ToString("MM/dd/yyyy-HH:mm:ss");
            return timeString;
        }

        public DateTime ParseCommentTime(string timeString)
        {
            DateTime time = DateTime.ParseExact(timeString, "MM/dd/yyyy-HH:mm:ss", null);
            return time;
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
                Content = commentDTO.Content,
                CreatedDate = FormatCommentTime(commentDTO.CreatedDate)
            };

            return commentModel;
        }

        public List<PostCardModel> MapPostCardDTOsToModels(List<PostCardDTO> postCardDTOs)
        {
            if (postCardDTOs == null)
            {
                return null;
            }

            List<PostCardModel> postCardModels = new List<PostCardModel>();

            foreach (PostCardDTO postCardDTO in postCardDTOs)
            {
                PostCardModel postCardModel = MapPostCardDTOToModel(postCardDTO);
                postCardModels.Add(postCardModel);
            }

            return postCardModels;
        }

        public PostCardModel MapPostCardDTOToModel(PostCardDTO postCardDTO)
        {
            if (postCardDTO == null)
            {
                return null;
            }

            CategoryDTO categoryDTO = postCardDTO.PostCategory;

            PostCardModel postCardModel = new PostCardModel()
            {
                PostId = postCardDTO.PostId,
                Title = postCardDTO.Title,
                ShortDescription = postCardDTO.ShortDescription,
                ThumbnailImageSrc = postCardDTO.ThumbnailImageSrc,
                CreatedDate = FormatPostDate(postCardDTO.CreatedDate),
                UpdatedDate = ComputeUpdateTime(postCardDTO.UpdatedDate),
                PostCategory = MapCategoryDTOToModel(categoryDTO),
                CommentCount = postCardDTO.CommentCount
            };

            return postCardModel;
        }

        public List<PaginationModel<PostCardModel>> MapPostCardPaginationDTOsToModels(List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs)
        {
            if (postCardPaginationDTOs == null)
            {
                return null;
            }

            List<PaginationModel<PostCardModel>> postCardPaginationModels = new List<PaginationModel<PostCardModel>>();

            foreach (PaginationDTO<PostCardDTO> postCardPaginationDTO in postCardPaginationDTOs)
            {
                PaginationModel<PostCardModel> postCardPaginationModel = MapPostCardPaginationDTOToModel(postCardPaginationDTO);
                postCardPaginationModels.Add(postCardPaginationModel);
            }

            return postCardPaginationModels;
        }

        public PaginationModel<PostCardModel> MapPostCardPaginationDTOToModel(PaginationDTO<PostCardDTO> postCardPaginationDTO)
        {
            if (postCardPaginationDTO == null)
            {
                return null;
            }

            PaginationModel<PostCardModel> postCardPaginationModel = new PaginationModel<PostCardModel>
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
                PostCategory = MapCategoryDTOToModel(postDTO.PostCategory),
                ShortDescription = postDTO.ShortDescription,
                ThumbnailImageSrc = postDTO.ThumbnailImageSrc,
                Content = postDTO.Content,
                CreatedDate = FormatPostDate(postDTO.CreatedDate),
                UpdatedDate = ComputeUpdateTime(postDTO.UpdatedDate),
                CommentModels = commentModels,
                CommentCount = postDTO.CommentCount
            };

            return postModel;
        }

        public PostModelWithPaginatedComments MapPostDTOToModelWithPaginatedComments(PostDTOWithPaginatedComments postDTOWithPaginatedComments)
        {
            if (postDTOWithPaginatedComments == null)
            {
                return null;
            }

            PostDTO postDTO = postDTOWithPaginatedComments.Post;
            PaginationDTO<CommentDTO> commentPaginationDTO = postDTOWithPaginatedComments.CommentPaginationDTO;

            PostModelWithPaginatedComments postModelWithPaginatedComments = new PostModelWithPaginatedComments()
            {
                Post = MapPostDTOToModel(postDTO),
                CommentPaginationModel = MapCommentPaginationDTOToModel(commentPaginationDTO)
            };

            return postModelWithPaginatedComments;
        }

        public PaginationModel<CommentModel> MapCommentPaginationDTOToModel(PaginationDTO<CommentDTO> commentPaginationDTO)
        {
            if (commentPaginationDTO == null)
            {
                return null;
            }

            List<CommentDTO> commentDTOs = commentPaginationDTO.DTOs;
            List<CommentModel> commentModels = MapCommentDTOsToModels(commentDTOs);

            PaginationModel<CommentModel> commentPaginationModel = new PaginationModel<CommentModel>()
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

        public UserDTO MapUserSignUpModelToDTO(UserSignUpModel userModel)
        {
            if (userModel == null)
            {
                return null;
            }

            UserDTO userDTO = new UserDTO()
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
                Password = userModel.Password
            };

            return userDTO;
        }

        public UserDTO MapUserLoginModelToDTO(UserLoginModel userModel)
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

        public UserDTO MapUserEditModelToDTO(UserEditModel userModel)
        {
            if (userModel == null)
            {
                return null;
            }

            UserDTO userDTO = new UserDTO()
            {
                Email = userModel.Email
            };

            return userDTO;
        }

        public UserEditModel MapUserEditDTOToModel(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            UserEditModel userModel = new UserEditModel()
            {
                Email = userDTO.Email
            };

            return userModel;
        }
    }
}