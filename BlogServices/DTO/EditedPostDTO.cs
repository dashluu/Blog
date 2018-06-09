using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class EditedPostDTO
    {
        public string Title { get; set; }
        public string PostCategory { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string ThumbnailImageSrc { get; set; }
    }
}
