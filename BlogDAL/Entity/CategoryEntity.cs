﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlogDAL.Entity
{
    public class CategoryEntity
    {
        [Key]
        public string CategoryId { get; set; }

        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public string Description { get; set; }
        public int PostCount { get; set; }
    }
}
