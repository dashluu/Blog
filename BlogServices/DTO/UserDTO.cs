﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool IsAdmin { get; set; }
    }
}