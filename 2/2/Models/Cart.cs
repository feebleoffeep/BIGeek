using System.Collections.Generic;
using System.Linq;

namespace _2.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // Додає товар до кошика або збільшує кількість існуючого товару
        public void AddItem(Product product, int quantity)
        {
            var existingItem = Items.FirstOrDefault(p => p.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem { Product = product, Quantity = quantity });
            }
        }

        // Повертає загальну вартість товарів у кошику
        public decimal GetTotalPrice()
        {
            return Items.Sum(i => (decimal)i.Product.Price * i.Quantity);
        }
    }
}







