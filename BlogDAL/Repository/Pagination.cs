using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Repository
{
    public class Pagination
    {
        public int PostPageSize { get; set; }
        public int CommentPageSize { get; set; }
        public int ChildCommentPageSize { get; set; }

        public Pagination()
        {
            PostPageSize = 20;
            CommentPageSize = 30;
            ChildCommentPageSize = 20;
        }
    }
}
