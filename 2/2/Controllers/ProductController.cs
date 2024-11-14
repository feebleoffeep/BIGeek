using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _2.Models;
using _2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower(); 

            products = products.ToList().Where(p =>
                p.Name.ToLower().Contains(searchTerm) ||
                (p.AlternativeNames != null && p.AlternativeNames
                    .ToLower().Split(',')
                    .Any(an => an.Trim().Contains(searchTerm)))
            ).AsQueryable();
        }

        ViewData["SearchTerm"] = searchTerm; 
        return View(products.ToList());
    }

    public IActionResult CreateProduct()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name"); 
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageProducts");
        }
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name"); 
        return View(product);
    }

    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId); 
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageProducts");
        }
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId); 
        return View(product);
    }

    public async Task<IActionResult> ByCategory(string category)
    {

        var products = await _context.Products
            .Include(c => c.Category) 
            .Where(p => p.Category.Name == category) 
            .ToListAsync();

        if (products == null || !products.Any())
        {
            return NotFound(); 
        }

        ViewBag.Category = category; 
        return View(products); 
    }

    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products
            .Include(c => c.Category) 
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product); 
    }

    [HttpPost, ActionName("DeleteProduct")]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ManageProducts"); 
    }
    [AllowAnonymous] 

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

        ViewBag.Query = query; 
        return View("SearchResults", filteredProducts);
    }

    [AllowAnonymous]
    public IActionResult AdvancedSearch(
    string query,
    string color,
    int? priceMin,
    int? priceMax,
    string screenSize, 
    int? ram,
    int? storage,
    string resolution)
{
    var products = _context.Products.AsQueryable();

    var colors = _context.Products.Select(p => p.Color).Distinct().ToList();
    var screenSizes = _context.Products.Select(p => p.ScreenDiagonal).Distinct().ToList();
    var rams = _context.Products.Select(p => p.RamSize).Distinct().ToList();
    var storages = _context.Products.Select(p => p.StorageSize).Distinct().ToList();
    var resolutions = _context.Products.Select(p => p.ScreenResolution).Distinct().ToList();

    ViewBag.Colors = colors;
    ViewBag.ScreenSizes = screenSizes;
    ViewBag.Rams = rams;
    ViewBag.Storages = storages;
    ViewBag.Resolutions = resolutions;

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

    ViewBag.Query = query;

    return View("SearchResults", products.ToList());
}



}