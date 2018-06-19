using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Models
{
    public struct Pagination
    {
        public int HomePostPageSize { get; set; }
        public int PostPageSize { get; set; }
        public int CommentPageSize { get; set; }
        public int ChildCommentPageSize { get; set; }

        public Pagination(int homePostPageSize, int postPageSize, int commentPageSize, int childCommentPageSize)
        {
            HomePostPageSize = homePostPageSize;
            PostPageSize = postPageSize;
            CommentPageSize = commentPageSize;
            ChildCommentPageSize = childCommentPageSize;
        }
    }
}
