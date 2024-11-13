using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _2.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва категорії є обов'язковою")]
        [StringLength(100, ErrorMessage = "Назва категорії не повинна перевищувати 100 символів")]
        public string? Name { get; set; }

        [ValidateNever]
        public virtual required ICollection<Product> Products { get; set; }
    }
}




