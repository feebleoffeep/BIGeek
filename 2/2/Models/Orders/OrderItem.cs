namespace _2.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Додана властивість Price для зберігання ціни товару
        public double Price { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}







