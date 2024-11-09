namespace _2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int OrderStatusId { get; set; }
        public int DeliveryMethodId { get; set; }
        public DateTime OrderDate { get; set; }
        public double DeliveryPrice { get; set; }  
        public string DeliveryTime { get; set; }

        public ApplicationUser User { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }

}






