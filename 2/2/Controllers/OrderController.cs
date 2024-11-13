using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    public async Task<IActionResult> MyOrders()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var orders = await _context.Orders
        .Where(o => o.UserId == userId)
        .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
        .Include(o => o.OrderStatus)
        .Include(o => o.DeliveryMethod)
        .ToListAsync();

        return View(orders);
    }

    // Метод для перегляду замовлень адміністратором
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ManageOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderStatus)
            .Include(o => o.DeliveryMethod)
            .ToListAsync();

        return View("ManageOrders", orders);
    }

    // Деталі конкретного замовлення
    public IActionResult OrderDetails(int orderId)
    {
        var order = _context.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderStatus)
            .Include(o => o.DeliveryMethod)
            .FirstOrDefault();

        if (order == null)
        {
            TempData["Error"] = "Замовлення не знайдено.";
            return RedirectToAction("MyOrders");
        }

        return View(order);
    }

    // Скасування замовлення
    [HttpPost]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null && order.OrderStatus?.Name != "Відхилено")
        {
            var declinedStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Name == "Відхилено");
            if (declinedStatus != null)
            {
                order.OrderStatus = declinedStatus;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Замовлення успішно скасовано.";
            }
            else
            {
                TempData["Error"] = "Статус 'Відхилено' не знайдено в базі даних.";
            }
        }
        else
        {
            TempData["Error"] = "Замовлення вже відхилено або не існує.";
        }

        return RedirectToAction("MyOrders");
    }

    // Видалення скасованого замовлення
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCancelledOrder(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null && order.OrderStatus?.Name == "Відхилено")
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Скасоване замовлення успішно видалено.";
        }
        else
        {
            TempData["Error"] = "Це замовлення не можна видалити.";
        }

        return RedirectToAction("ManageOrders");
    }

    // Відправка замовлення
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SendOrder(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null && order.OrderStatus?.Name == "Очікується")
        {
            var sentStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Name == "Відправлено");
            if (sentStatus != null)
            {
                order.OrderStatus = sentStatus;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Замовлення відправлено.";
            }
            else
            {
                TempData["Error"] = "Статус 'Відправлено' не знайдено в базі даних.";
            }
        }
        else
        {
            TempData["Error"] = "Замовлення не можна відправити.";
        }

        return RedirectToAction("OrderDetails", new { orderId });
    }

    // Підтвердження замовлення
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ConfirmOrder(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null && order.OrderStatus?.Name == "Очікується")
        {
            var confirmedStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Name == "Підтверджено");
            if (confirmedStatus != null)
            {
                order.OrderStatus = confirmedStatus;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Замовлення підтверджено.";
            }
            else
            {
                TempData["Error"] = "Статус 'Підтверджено' не знайдено в базі даних.";
            }
        }
        else
        {
            TempData["Error"] = "Замовлення не можна підтвердити.";
        }

        return RedirectToAction("ManageOrders", new { orderId });
    }
}
