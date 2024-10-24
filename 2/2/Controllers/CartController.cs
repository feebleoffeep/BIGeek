using Microsoft.AspNetCore.Mvc;
using _2.Models;
using Newtonsoft.Json;
using _2.Data;

namespace _2.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод для отримання кошика з сесії
        private List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetString("Cart");
            return cart == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        // Метод для збереження кошика в сесію
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // GET: Cart/Index
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Додати продукт до кошика
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();

            // Шукаємо, чи вже є такий продукт у кошику
            var existingItem = cart.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // Якщо продукт вже є, збільшуємо кількість
                existingItem.Quantity++;
            }
            else
            {
                // Якщо продукту немає, додаємо його як новий елемент кошика
                var cartItem = new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                };

                cart.Add(cartItem);
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }


        // Видалити продукт з кошика
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }


        // Оновити кількість товарів у кошику
        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }


        // Очистити кошик
        public IActionResult ClearCart()
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction("Index");
        }
    }
}







