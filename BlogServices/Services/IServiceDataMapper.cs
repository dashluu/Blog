using BlogDAL.Entity;
using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface IServiceDataMapper
    {
        List<CommentDTO> MapCommentEntitiesToDTOs(List<CommentEntity> commentEntities);
        PostDTO MapPostEntityToDTO(PostEntity postEntity);
        PostEntity MapEditedPostDTOToEntity(EditedPostDTO editedPostDTO);
        PostCardDTO MapPostCardEntityToDTO(PostEntity postEntity);
        CommentDTO MapCommentEntityToDTO(CommentEntity commentEntity);
    }
}
