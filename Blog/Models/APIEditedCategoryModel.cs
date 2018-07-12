using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class APIEditedCategoryModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}