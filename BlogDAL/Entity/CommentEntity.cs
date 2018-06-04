using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Entity
{
    public class CommentEntity
    {
        public string CommentId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public List<CommentEntity> ChildCommentEntities { get; set; }
    }
}
