using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Entity
{
    public class PaginationEntity<T>
    {
        public List<T> Entities { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Pages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
    }
}
