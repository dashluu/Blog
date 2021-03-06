﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class APIPostCardModel
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbnailImageSrc { get; set; }
        public APIPostCategoryModel PostCategory { get; set; }
        public int CommentCount { get; set; }
    }
}