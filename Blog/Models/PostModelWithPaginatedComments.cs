using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class PostModelWithPaginatedComments
    {
        public PostModel Post { get; set; }
        public PaginationModel<CommentModel> CommentPaginationModel { get; set; }
    }
}