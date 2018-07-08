using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class PaginationModel<T>
    {
        public List<T> Models { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Pages { get; set; }
    }
}