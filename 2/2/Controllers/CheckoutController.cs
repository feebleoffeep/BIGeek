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
    public async Task<IActionResult> PlaceOrder(int deliveryMethodId, string cityName, float? distance)
    {
        var cart = GetCart();

        if (!cart.Any())
        {
            ModelState.AddModelError("", "Ваш кошик порожній.");
            return RedirectToAction("Index", "Cart");
        }

        // Розрахунок відстані за вибраним містом
        var selectedCity = _context.Cities.FirstOrDefault(c => c.Name == cityName);
        double distanceToCity = 0;

        if (selectedCity != null)
        {
            distanceToCity = GetDistance(49.4673, 27.5161, selectedCity.Latitude, selectedCity.Longitude);
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

        if (selectedDeliveryMethod.Name == "Кур'єр" && distanceToCity > 0)
        {
            deliveryPrice = distanceToCity * 5; // Ціна доставки
            deliveryTime = (distanceToCity / 50).ToString("F1") + " год."; // Час доставки
        }
        else if (selectedDeliveryMethod.Name == "Самовивіз")
        {
            deliveryPrice = 0;
            deliveryTime = "Не потрібно";
        }

        // Створення нового замовлення
        var order = new Order
        {
            OrderDate = DateTime.Now,
            DeliveryMethodId = deliveryMethodId,
            DeliveryPrice = deliveryPrice,
            DeliveryTime = deliveryTime,
            OrderItems = cart.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        // Збереження замовлення у базу даних
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Очищення кошика
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

    public double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Рadius of Earth in km
        var lat1Rad = ToRadians(lat1);
        var lon1Rad = ToRadians(lon1);
        var lat2Rad = ToRadians(lat2);
        var lon2Rad = ToRadians(lon2);

        var dlat = lat2Rad - lat1Rad;
        var dlon = lon2Rad - lon1Rad;

        var a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distance = R * c; // Відстань в км
        return distance;
    }

    private double ToRadians(double degree)
    {
        return degree * (Math.PI / 180);
    }

}


