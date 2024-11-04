using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace _2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            List<Category> categories = _context.Categories.ToList(); // Отримуємо категорії

            ViewBag.Categories = categories; // Зберігаємо категорії у ViewBag

            return View(products);
        }



        [AllowAnonymous]
        [Route("/NotFound")]

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
