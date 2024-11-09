using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _2.Models;
using System.Linq;
using System.Threading.Tasks;
using _2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Tsp;
using Microsoft.IdentityModel.Tokens;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ManageProducts(string searchTerm)
    {
        var products = _context.Products.Include(c => c.Category).AsQueryable();

        // Якщо введено термін пошуку, фільтруємо продукти
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower(); // Переводимо пошуковий термін у нижній регістр

            // Завантажуємо продукти в пам'ять, щоб обробити альтернативні назви
            products = products.ToList().Where(p =>
                p.Name.ToLower().Contains(searchTerm) ||
                (p.AlternativeNames != null && p.AlternativeNames
                    .ToLower().Split(',')
                    .Any(an => an.Trim().Contains(searchTerm)))
            ).AsQueryable();
        }

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
    // Видалення продукту (Підтвердження видалення)
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products
            .Include(c => c.Category) // Включаємо категорію для відображення
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product); // Повертаємо до представлення для підтвердження видалення
    }

    // Видалення продукту (Обробка запиту)
    [HttpPost, ActionName("DeleteProduct")]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ManageProducts"); // Повернення до списку продуктів після видалення
    }
    [AllowAnonymous] // Дозволяє доступ для всіх користувачів, не тільки для адміністраторів

    public IActionResult Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View("SearchResults", new List<Product>());
        }

        var products = _context.Products
            .Include(p => p.Category)
            .ToList();

        var lowerCaseQuery = query.ToLower();

        var filteredProducts = products
            .Where(p =>
                p.Name != null && p.Name.ToLower().Contains(lowerCaseQuery) ||
                (p.AlternativeNames != null &&
                 p.AlternativeNames
                    .Split(',')
                    .Select(altName => altName.Trim().ToLower())
                    .Any(name => name.Contains(lowerCaseQuery)))
            )
            .ToList();

        ViewBag.Query = query; // Додаємо запит до ViewBag
        return View("SearchResults", filteredProducts);
    }

    [AllowAnonymous]
    public IActionResult AdvancedSearch(
    string query,
    string color,
    int? priceMin,
    int? priceMax,
    string screenSize, // Тип double для зберігання розміру екрану
    int? ram,
    int? storage,
    string resolution)
{
    // Ініціалізуємо запит до бази даних
    var products = _context.Products.AsQueryable();

    // Отримуємо унікальні значення для фільтрів
    var colors = _context.Products.Select(p => p.Color).Distinct().ToList();
    var screenSizes = _context.Products.Select(p => p.ScreenDiagonal).Distinct().ToList();
    var rams = _context.Products.Select(p => p.RamSize).Distinct().ToList();
    var storages = _context.Products.Select(p => p.StorageSize).Distinct().ToList();
    var resolutions = _context.Products.Select(p => p.ScreenResolution).Distinct().ToList();

    // Передаємо значення для фільтрів у ViewBag
    ViewBag.Colors = colors;
    ViewBag.ScreenSizes = screenSizes;
    ViewBag.Rams = rams;
    ViewBag.Storages = storages;
    ViewBag.Resolutions = resolutions;

    // Фільтруємо за запитом, кольором, ціною, екраном і т.д.
    if (!string.IsNullOrEmpty(query))
    {
        var searchQuery = query.ToLower();
        products = products
            .AsEnumerable()
            .Where(p =>
                (p.Name != null && p.Name.ToLower().Contains(searchQuery)) ||
                (p.AlternativeNames != null &&
                 p.AlternativeNames
                    .Split(',', StringSplitOptions.None)
                    .Select(altName => altName.Trim().ToLower())
                    .Contains(searchQuery))
            ).AsQueryable();
    }

    if (!string.IsNullOrEmpty(color))
    {
        products = products.Where(p => p.Color.Equals(color, StringComparison.OrdinalIgnoreCase));
    }

    if (priceMin.HasValue)
    {
        products = products.Where(p => p.Price >= priceMin.Value);
    }
    if (priceMax.HasValue)
    {
        products = products.Where(p => p.Price <= priceMax.Value);
    }

        if (!string.IsNullOrEmpty(screenSize))
        {
            products = products.Where(p => p.ScreenDiagonal.ToString() == screenSize);
        }

        if (ram.HasValue)
    {
        products = products.Where(p => p.RamSize == ram.Value);
    }

    if (storage.HasValue)
    {
        products = products.Where(p => p.StorageSize == storage.Value);
    }

    if (!string.IsNullOrEmpty(resolution))
    {
        products = products.Where(p => p.ScreenResolution.Contains(resolution, StringComparison.OrdinalIgnoreCase));
    }

    // Передаємо результат в представлення
    ViewBag.Query = query;

    return View("SearchResults", products.ToList());
}



}