using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class DataAPIModel
    {
        public string Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}