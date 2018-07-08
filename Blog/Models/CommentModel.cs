using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class CommentModel
    {
        public string CommentId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreatedDate { get; set; }
        public List<CommentModel> ChildCommentModels { get; set; }
    }
}