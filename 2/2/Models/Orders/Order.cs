using _2.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderStatusId { get; set; }
        public int DeliveryMethodId { get; set; }
        public DateTime OrderDate { get; set; }
        public double DeliveryPrice { get; set; }
        public string? DeliveryTime { get; set; }

        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public virtual OrderStatus? OrderStatus { get; set; }
        public virtual DeliveryMethod? DeliveryMethod { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }

    }
}






