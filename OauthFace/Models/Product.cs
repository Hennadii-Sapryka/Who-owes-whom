using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WhoWhom.Models
{
    public class Product
    {
        [Display(Name = "№")]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Що купив")]
        public string Name { get; set; }

        [Required]
        //[MaxLength(10)]
        [Range(1, int.MaxValue, ErrorMessage = "лише числа які більше 0")]
        [Display(Name = "Ціна")]
        public int Price { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Хто купив")]
        public string User { get; set; }

        //public List<AccountUser> accountUsers { get; set; }
    }
}
