using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _2.Models;
using System.Linq;
using System.Threading.Tasks;
using _2.Data;

[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Список продуктів
public IActionResult ManageProducts(string searchTerm)
    {
        // Отримуємо всі продукти з бази даних
        var products = _context.Products.AsQueryable();

        // Якщо введено термін пошуку, фільтруємо продукти
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            products = products.Where(p => p.Name.Contains(searchTerm));
        }

        // Повертаємо відфільтровані продукти до представлення
        ViewData["SearchTerm"] = searchTerm; // Зберігаємо термін для відображення в полі вводу
        return View(products.ToList());
    }

    // Створення продукту (Форма)
    public IActionResult CreateProduct()
    {
        return View();
    }

    // Створення продукту (Дані)
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageProducts");
        }
        return View(product);
    }

    // Оновлення продукту (Форма)
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // Оновлення продукту (Дані)
    [HttpPost]
    public async Task<IActionResult> EditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageProducts");
        }
        return View(product);
    }

    // Видалення продукту
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("ManageProducts");
    }
  
}





