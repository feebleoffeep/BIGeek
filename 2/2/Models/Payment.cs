using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _2.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string ?PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }



        [ValidateNever]
        public virtual ICollection<Product> Products { get; set; }
    }
}







