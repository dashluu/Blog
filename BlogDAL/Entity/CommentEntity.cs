using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlogDAL.Entity
{
    public class CommentEntity
    {
        [Key]
        public string CommentId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public List<CommentEntity> ChildCommentEntities { get; set; }
        public PostEntity Post { get; set; }
        public CommentEntity RootComment { get; set; }
    }
}
