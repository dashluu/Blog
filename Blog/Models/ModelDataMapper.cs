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
            Func<CategoryDTO, CategoryModel> delegateMapper = MapCategoryDTOToModel;
            List<CategoryModel> categoryModels = MapGenericList(categoryDTOs, delegateMapper);

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
                Func<CommentDTO, CommentModel> delegateMapper = MapCommentDTOToModel;
                List<CommentModel> childCommentModels = MapGenericList(childCommentDTOs, delegateMapper);
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
            Func<PostCardDTO, PostCardModel> delegateMapper = MapPostCardDTOToModel;
            List<PostCardModel> postCardModels = MapGenericList(postCardDTOs, delegateMapper);

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
            Func<PaginationDTO<PostCardDTO>, PaginationModel<PostCardModel>> delegateMapper = MapPostCardPaginationDTOToModel;
            List<PaginationModel<PostCardModel>> postCardPaginationModels = MapGenericList(postCardPaginationDTOs, delegateMapper);

            return postCardPaginationModels;
        }

        public PaginationModel<PostCardModel> MapPostCardPaginationDTOToModel(PaginationDTO<PostCardDTO> postCardPaginationDTO)
        {
            Func<List<PostCardDTO>, List<PostCardModel>> delegateMapper = MapPostCardDTOsToModels;
            PaginationModel<PostCardModel> postCardPaginationModel = MapGenericPagination(postCardPaginationDTO, delegateMapper);

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
            Func<List<CommentDTO>, List<CommentModel>> delegateMapper = MapCommentDTOsToModels;
            PaginationModel<CommentModel> commentPaginationModel = MapGenericPagination(commentPaginationDTO, delegateMapper);

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

        public PaginationModel<T2> MapGenericPagination<T1, T2>(PaginationDTO<T1> pagination1, Func<List<T1>, List<T2>> delegateMapper)
        {
            if (pagination1 == null)
            {
                return null;
            }

            PaginationModel<T2> paginationModel = new PaginationModel<T2>
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