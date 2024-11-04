using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _2.Models;
using System.Linq;
using System.Threading.Tasks;
using _2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        var products = _context.Products.Include(c => c.Category).AsQueryable();

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
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name"); // Завантаження категорій
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
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name"); // Перезавантаження категорій у разі помилки
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
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId); // Встановлення вибраної категорії
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
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId); // Перезавантаження категорій у разі помилки
        return View(product);
    }
    // Метод для відображення продуктів за категорією
    public async Task<IActionResult> ByCategory(string category)
    {
        // Отримуємо продукти, що належать до вказаної категорії
        var products = await _context.Products
            .Include(c => c.Category) // Включаємо категорії для відображення
            .Where(p => p.Category.Name == category) // Фільтруємо за назвою категорії
            .ToListAsync();

        if (products == null || !products.Any())
        {
            return NotFound(); // Якщо не знайдено, повертаємо NotFound
        }

        ViewBag.Category = category; // Зберігаємо назву категорії для відображення в заголовку
        return View(products); // Повертаємо знайдені продукти до представлення
    }

}