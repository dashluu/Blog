using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class CommentDTO
    {
        public string CommentId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CommentDTO> ChildCommentDTOs { get; set; }
    }
}
