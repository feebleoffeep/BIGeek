using System.Diagnostics;
using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


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
            List<Category> categories = _context.Categories.ToList(); 

            ViewBag.Categories = categories; 

            return View(products);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string? returnUrl = null)
        {
            // Зчитуємо попередню мову з файлу cookie
            var previousCulture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            if (previousCulture != culture)
            {
                // Записуємо в журнал, якщо мова змінилася
                Debug.WriteLine($"Language change detected: {previousCulture} -> {culture}");
            }

            // Оновлюємо значення мови в cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return returnUrl != null ? LocalRedirect(returnUrl) : RedirectToAction("Index");
        }

        [AllowAnonymous]
        [Route("/NotFound")]

        public IActionResult PageNotFound()
        {
            return View();
        }

    }
}

