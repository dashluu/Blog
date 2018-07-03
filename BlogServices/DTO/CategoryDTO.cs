using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class CategoryDTO
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PostCount { get; set; }
    }
}
