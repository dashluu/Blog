using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public class HashService : IHashService
    {
        public string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
