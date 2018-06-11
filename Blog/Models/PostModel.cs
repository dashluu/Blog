using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class PostModel
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public string PostCategory { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string ThumbnailImageSrc { get; set; }
        public List<CommentModel> CommentModels { get; set; }
    }
}