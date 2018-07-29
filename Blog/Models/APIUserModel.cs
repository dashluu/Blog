using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class APIUserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool IsAdmin { get; set; }
    }
}