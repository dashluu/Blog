﻿using BlogDAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BlogDAL.Repository
{
    public interface IPostRepository : IBaseRepository<PostEntity>
    {
        List<PostEntity> GetPostEntitiesWithCategory(string category);
        (PostEntity postEntity, bool end) GetPostEntityWithCommentPagination(string id);
        PostEntity GetPostEntity(string id);
    }
}