using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class EditedPostModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public CategoryModel PostCategory { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string ThumbnailImageSrc { get; set; }
    }
}