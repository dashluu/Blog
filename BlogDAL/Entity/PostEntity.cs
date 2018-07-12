using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlogDAL.Entity
{
    public class PostEntity
    {
        [Key]
        public string PostId { get; set; }
        public string Title { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryEntity PostCategory { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string ThumbnailImageSrc { get; set; }

        public List<CommentEntity> CommentEntities { get; set; }
        public int CommentCount { get; set; }
        public string CategoryId { get; set; }
    }
}
