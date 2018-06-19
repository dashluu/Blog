using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class PaginationDTO<T>
    {
        public List<T> DTOs { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public int Pages { get; set; }
    }
}
