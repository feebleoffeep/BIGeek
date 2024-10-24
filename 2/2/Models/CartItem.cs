namespace _2.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }  // Відповідає Id продукту
        public string Name { get; set; }     // Назва продукту
        public double Price { get; set; }    // Ціна продукту
        public int Quantity { get; set; }    // Кількість продукту

        // Можна також додати посилання на сам продукт, якщо це потрібно
        public Product Product { get; set; }
    }
}







