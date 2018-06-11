using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public interface IModelDataMapper
    {
        List<CommentModel> MapCommentDTOsToModels(List<CommentDTO> commentDTOs);
        PostModel MapPostDTOToModel(PostDTO postDTO);
        EditedPostDTO MapEditedPostModelToDTO(EditedPostModel editedPostModel);
        PostCardModel MapPostCardDTOToModel(PostCardDTO postCardDTO);
        CommentModel MapCommentDTOToModel(CommentDTO commentDTO);
    }
}