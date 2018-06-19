using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Entity
{
    public class PostEntityWithPaginatedComments
    {
        public PostEntity Post { get; set; }
        public PaginationEntity<CommentEntity> CommentPaginationEntity { get; set; }
    }
}
