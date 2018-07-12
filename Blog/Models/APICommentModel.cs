using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class APICommentModel
    {
        public string CommentId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public List<APICommentModel> ChildCommentModels { get; set; }
    }
}