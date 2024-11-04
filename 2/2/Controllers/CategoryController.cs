using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context; // Додайте ваш контекст бази даних
    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Дія для відображення всіх категорій
    public IActionResult ManageCategories()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    // Дія для відображення форми додавання нової категорії
    public IActionResult CreateCategory()
    {
        return View();
    }

    // Дія для обробки POST-запиту на додавання нової категорії
    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("ManageCategories");
        }

        return View(category); // Якщо не валідно, повертаємо ту ж саму форму
    }

    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageCategories"); // Змінено з ManageProducts на ManageCategories
        }
        return View(category);
    }

    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("ManageCategories");
    }
}

