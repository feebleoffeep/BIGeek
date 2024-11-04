using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _2.Models
{

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }


        [ValidateNever]
        public virtual ICollection<Product> Products { get; set; }
    }
}



