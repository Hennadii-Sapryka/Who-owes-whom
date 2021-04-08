﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WhoOwesWhom.Models
{
    public class Product
    {

        [Display(Name = "№")]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "What bought")]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(10)]
        [Range(1, int.MaxValue, ErrorMessage = "лише числа які більше 0")]
        [Display(Name = "Price")]
        public int Price { get; set; }

        public string UserName { get; set; }

        //[Required]
        //[MaxLength(10)]
        //[Display(Name = "Who bought")]
        //public string User { get; set; }
    }
}
