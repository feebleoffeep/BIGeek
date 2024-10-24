using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Налаштування підключення до бази даних
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Додаємо сервіс Identity з підтримкою ролей та налаштуваннями паролів
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Змінюємо вимоги до паролів
    options.Password.RequireDigit = false; // Вимога на наявність цифри
    options.Password.RequireLowercase = false; // Вимога на наявність маленької літери
    options.Password.RequireUppercase = false; // Вимога на наявність великої літери
    options.Password.RequireNonAlphanumeric = false; // Вимога на наявність небуквенно-цифрового символу
    options.Password.RequiredLength = 6; // Мінімальна довжина пароля
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Додаємо сесію
builder.Services.AddSession();

// Оновлюємо сторінку не перезапускаючи проект
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Додаємо контролери з представленнями
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Перехоплення 404 помилок і перенаправлення на NotFound сторінку
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/NotFound";
        await next();
    }
});

app.UseStaticFiles();

// Використовуємо сесію
app.UseSession();

// Налаштування маршрутизації
app.UseRouting();
app.UseAuthentication(); // Додано аутентифікацію
app.UseAuthorization();  // Додано авторизацію

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Ініціалізація бази даних з міграціями і seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Застосування міграцій до бази даних
        await DbInitializer.Seed(services); // Seed метод для додавання ролей та адміністратора
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during seeding the DB.");
    }
}

// Запуск програми
app.Run();
