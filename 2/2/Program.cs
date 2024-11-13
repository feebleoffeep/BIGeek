using _2.Data;
using _2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Налаштування бази даних
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString: builder.Configuration.GetConnectionString("DefaultConnection")));

// Налаштування локалізації
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultureCodes = new List<string> { "en-US", "uk-UA" };
var supportedCultures = supportedCultureCodes.Select(culture => new System.Globalization.CultureInfo(culture)).ToList();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("uk-UA");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Налаштування Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8; // Мінімальна довжина пароля
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Налаштування сесій
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Сесія завершиться після 30 хв бездіяльності
    options.Cookie.HttpOnly = true; // Захист від доступу до куків через JavaScript
    options.Cookie.IsEssential = true; // Куки є обов’язковими
});

// Реєстрація LanguageService
//builder.Services.AddScoped<LanguageService>();

// // Налаштування Razor та локалізації для Views
// builder.Services.AddControllersWithViews()
//     .AddRazorRuntimeCompilation() // Використовувати тільки під час розробки
//     .AddViewLocalization();

var app = builder.Build();

// Обробка статусу 404
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404 && !context.Request.Path.Value.Contains("/NotFound"))
    {
        context.Request.Path = "/NotFound";
        context.Response.StatusCode = 200; // Повертаємо 200, щоб уникнути циклів
        await next();
    }
});

// Використання статичних файлів
app.UseStaticFiles();

// Використання сесій
app.UseSession();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);
// Маршрутизація
app.UseRouting();

// Аутентифікація та авторизація
app.UseAuthentication();
app.UseAuthorization();

// Налаштування маршрутів
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Ініціалізація бази даних
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync(); // Асинхронна міграція
        await DbInitializer.Seed(services); // Ініціалізація даних
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during database initialization.");
    }
}

app.Run();

