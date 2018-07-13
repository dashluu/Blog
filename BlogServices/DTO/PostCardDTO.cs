using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class PostCardDTO
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbnailImageSrc { get; set; }
        public CategoryDTO PostCategory { get; set; }
        public int CommentCount { get; set; }
    }
}
