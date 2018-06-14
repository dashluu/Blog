﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlogDAL.Entity
{
    public class CategoryEntity
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Statistics { get; set; }
    }
}