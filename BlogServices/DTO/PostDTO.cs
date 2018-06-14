using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class PostDTO
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public CategoryDTO PostCategory { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string ThumbnailImageSrc { get; set; }
        public List<CommentDTO> CommentDTOs { get; set; }
    }
}
