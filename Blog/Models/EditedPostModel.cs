using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class EditedPostModel
    {
        public string Title { get; set; }
        public CategoryModel PostCategory { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string ThumbnailImageSrc { get; set; }
    }
}