using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.DTO
{
    public class AuthDTO
    {
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
