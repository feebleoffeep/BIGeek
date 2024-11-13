using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> userManager;

    public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        this.userManager = userManager;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var cart = GetCart();
        if (!cart.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        var userId = userManager.GetUserId(User);
        var user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
        }

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

        var selectedDeliveryMethod = _context.DeliveryMethods.FirstOrDefault(m => m.Id == deliveryMethodId);
        if (selectedDeliveryMethod == null)
        {
            ModelState.AddModelError("", "Невірно вибраний метод доставки.");
            return RedirectToAction("Index");
        }

        double deliveryPrice = 0;
        string deliveryTime = "";

        if (selectedDeliveryMethod.Id == 2 && distance.HasValue)
        {
            deliveryPrice = distance.Value * 5;
            double deliveryTimeInHours = distance.Value / 50;
            int hours = (int)deliveryTimeInHours;
            int minutes = (int)((deliveryTimeInHours - hours) * 60);
            deliveryTime = $"{hours} год. {minutes} хв.";
        }
        else if (selectedDeliveryMethod.Id == 1)
        {
            deliveryPrice = 0;
            deliveryTime = "Не потрібно";
        }

        var defaultOrderStatusId = 1;
        var userId = userManager.GetUserId(User);
        var user = await userManager.FindByIdAsync(userId);

        var order = new Order
        {
            UserId = userId,
            FirstName = user?.FirstName,
            LastName = user?.LastName,
            OrderDate = DateTime.Now,
            DeliveryMethodId = deliveryMethodId,
            DeliveryPrice = deliveryPrice,
            DeliveryTime = deliveryTime,
            OrderStatusId = defaultOrderStatusId,
            OrderItems = cart.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        SaveCart(new List<CartItem>());

        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
    }


    public IActionResult OrderConfirmation(int orderId)
    {
        var order = _context.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.OrderItems).ThenInclude(o => o.Product)
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










