using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class CommentModel
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public CommentModel RootComment { get; set; }
    }
}