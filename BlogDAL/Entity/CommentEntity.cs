﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlogDAL.Entity
{
    public class CommentEntity
    {
        [Key]
        public string CommentId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CommentEntity> ChildCommentEntities { get; set; }
        public PostEntity Post { get; set; }
        public CommentEntity ParentComment { get; set; }

        [ForeignKey("Post")]
        public string PostId { get; set; }
        [ForeignKey("ParentComment")]
        public string ParentCommentId { get; set; }
    }
}
