using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using _2.Models;
using _2.Data;

public class ProductControllerTests
{
    private Mock<ApplicationDbContext> GetMockDbContext()
    {
        var mockSet = new Mock<DbSet<Product>>();
        var mockCategorySet = new Mock<DbSet<Category>>();

        var category = new Category { Id = 6, Name = "Smartphones" };
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "iPhone 14", Category = category },
            new Product { Id = 2, Name = "Samsung Galaxy S23", Category = category }
        }.AsQueryable();

        mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
        mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
        mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
        mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

        var categoryList = new List<Category> { category }.AsQueryable();
        mockCategorySet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(categoryList.Provider);
        mockCategorySet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(categoryList.Expression);
        mockCategorySet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(categoryList.ElementType);
        mockCategorySet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(categoryList.GetEnumerator());

        var mockContext = new Mock<ApplicationDbContext>();
        mockContext.Setup(c => c.Products).Returns(mockSet.Object);
        mockContext.Setup(c => c.Categories).Returns(mockCategorySet.Object);

        return mockContext;
    }

    [Fact]
    public void ManageProducts_ReturnsProducts_WhenSearchIsEmpty()
    {
        var mockContext = GetMockDbContext();
        var controller = new ProductController(mockContext.Object);

        var result = controller.ManageProducts("") as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<List<Product>>(result.Model);
        Assert.Equal(2, model.Count); 
    }

    [Fact]
    public void ManageProducts_FiltersProducts_BySearchTerm()
    {
        var mockContext = GetMockDbContext();
        var controller = new ProductController(mockContext.Object);

        var result = controller.ManageProducts("iPhone") as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<List<Product>>(result.Model);
        Assert.Single(model); 
        Assert.Equal("iPhone 14", model.First().Name);
    }

    [Fact]
    public void ManageProducts_ReturnsEmptyList_WhenNoProductMatchesSearch()
    {
        var mockContext = GetMockDbContext();
        var controller = new ProductController(mockContext.Object);

        var result = controller.ManageProducts("Nokia") as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<List<Product>>(result.Model);
        Assert.Empty(model); 
    }
}
