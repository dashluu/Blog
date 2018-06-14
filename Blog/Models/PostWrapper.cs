using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class PostWrapper
    {
        public PostModel Post { get; set; }
        public bool End { get; set; }
    }
}