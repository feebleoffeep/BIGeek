using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;

    public CheckoutController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var cart = GetCart();
        if (!cart.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        // Отримання всіх доступних методів доставки
        ViewBag.DeliveryMethods = _context.DeliveryMethods.ToList();

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(int deliveryMethodId, float? distance)
    {
        var cart = GetCart();

        if (!cart.Any())
        {
            ModelState.AddModelError("", "Ваш кошик порожній.");
            return RedirectToAction("Index", "Cart");
        }

        // Отримання вибраного методу доставки
        var selectedDeliveryMethod = _context.DeliveryMethods.FirstOrDefault(m => m.Id == deliveryMethodId);

        if (selectedDeliveryMethod == null)
        {
            ModelState.AddModelError("", "Невірно вибраний метод доставки.");
            return RedirectToAction("Index");
        }

        // Ініціалізація змінних для ціни та часу доставки
        double deliveryPrice = 0;
        string deliveryTime = "";

        // Розрахунок ціни і часу доставки для кур'єра
        if (selectedDeliveryMethod.Id == 2 && distance.HasValue) // ID кур'єра = 2
        {
            deliveryPrice = distance.Value * 5; // Ціна доставки: 5 грн за км
            deliveryTime = (distance.Value / 50).ToString("F1") + " год."; // Час доставки: 1 година на 50 км
        }
        else if (selectedDeliveryMethod.Id == 1) // Самовивіз
        {
            deliveryPrice = 0;
            deliveryTime = "Не потрібно";
        }

        // Створення нового замовлення
        var order = new Order
        {
            OrderDate = DateTime.Now,
            DeliveryMethodId = deliveryMethodId, // Збереження вибраного методу доставки
            DeliveryPrice = deliveryPrice,       // Збереження ціни доставки
            DeliveryTime = deliveryTime,         // Збереження часу доставки
            OrderItems = cart.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        // Збереження замовлення в базу даних
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Очищення кошика після успішного замовлення
        SaveCart(new List<CartItem>());

        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
    }

    public IActionResult OrderConfirmation(int orderId)
    {
        var order = _context.Orders
            .Where(o => o.Id == orderId)
            .FirstOrDefault();

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    private List<CartItem> GetCart()
    {
        // Використовуємо сесію для отримання кошика
        var cart = HttpContext.Session.GetString("Cart");
        return cart == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cart);
    }

    private void SaveCart(List<CartItem> cart)
    {
        // Збереження кошика в сесію
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }
}





