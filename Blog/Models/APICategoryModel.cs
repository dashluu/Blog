using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class APICategoryModel
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PostCount { get; set; }
    }
}