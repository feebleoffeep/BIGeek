using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Mysqlx.Crud;

namespace _2.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }

}







