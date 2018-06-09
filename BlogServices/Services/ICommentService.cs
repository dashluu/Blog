using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface ICommentService
    {
        string AddCommentDTO(string postId, string commentContent, string username);
        bool AddChildCommentDTO(string postId, string commentId, string commentContent, string username);
    }
}
