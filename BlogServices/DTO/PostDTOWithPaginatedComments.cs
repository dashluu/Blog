using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class PostDTOWithPaginatedComments
    {
        public PostDTO Post { get; set; }
        public PaginationDTO<CommentDTO> CommentPaginationDTO { get; set; }
    }
}
