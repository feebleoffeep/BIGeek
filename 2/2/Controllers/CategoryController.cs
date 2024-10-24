using _2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CategoryController : Controller
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context; // Додайте ваш контекст бази даних
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Дія для відображення всіх категорій
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList(); // Передбачаємо, що у вас є DbSet<Category>
            return View(categories);
        }

        // Дія для відображення товарів в обраній категорії
        public IActionResult ProductsByCategory(int id)
        {
            var category = _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category.Products); // Передаємо список товарів у вибраній категорії
        }
    }
}





