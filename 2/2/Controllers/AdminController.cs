using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using _2.Models;
using Microsoft.EntityFrameworkCore;
using _2.Data;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult AdminDashboard()
    {
        return View(); 
    }

    [HttpGet]
    public async Task<IActionResult> ManageUsers(string searchTerm)
    {
        var users = await _userManager.Users.ToListAsync();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var searchTerms = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            users = users.Where(u => searchTerms.All(term =>
                u.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                u.LastName.Contains(term, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        ViewData["SearchTerm"] = searchTerm; 
        return View(users);
    }
    
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(string id, ApplicationUser user)
    {
        if (ModelState.IsValid)
        {
            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return NotFound();
            }

            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;

            var result = await _userManager.UpdateAsync(dbUser);
            if (result.Succeeded)
            {
                return RedirectToAction("ManageUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction("ManageUsers");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View("EditUser", user);
    }
}
