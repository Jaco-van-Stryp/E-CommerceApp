using Bogus;
using E_Commerce.Data;
using E_Commerce.Features.Products.GetAllProducts;
using E_CommerceTests.Factory;
using FluentAssertions;

namespace E_CommerceTests.Features.Products.GetAllProducts;

public class GetAllProductsTest
{
    private readonly AppDbContext _database = DbContextFactory.CreateDatabase();

    private async Task SeedDatabase(string? category = null)
    {
        var faker = new Faker();
        var useRandomCategory = false;
        List<E_Commerce.Data.Entities.Products> products = [];
        for (var i = 0; i < 1000; i++)
        {
            var categoryToUse = faker.Commerce.Categories(1)[0];
            if (category != null)
            {
                useRandomCategory = !useRandomCategory;
                if (useRandomCategory)
                {
                    categoryToUse = category;
                }
            }

            var product = new E_Commerce.Data.Entities.Products
            {
                Id = faker.Random.Guid(),
                ProductCategory = categoryToUse,
                ProductName = faker.Commerce.ProductName(),
                ProductDescription = faker.Commerce.ProductDescription(),
                ProductImageFileName = faker.Random.Guid(),
                ProductPrice = decimal.Parse(faker.Commerce.Price()),
                Active = faker.Random.Bool(),
            };
            products.Add(product);
        }
        await _database.Products.AddRangeAsync(products);
        await _database.SaveChangesAsync();
    }

    [Fact]
    public async Task Given_All_Products_Without_Category_When_Fetching_All_Products_Then_Return_All_Products_Unfiltered()
    {
        await SeedDatabase();
        var query = new GetAllProductsQuery();
        var handler = new GetAllProductsHandler(_database);
        var result = await handler.Handle(query, CancellationToken.None);
        result.Should().NotBeNull();
        result.Should().HaveCount(1000);
        result[0].Should().BeOfType<GetAllProductsResults>();
    }

    [Fact]
    public async Task Given_All_Products_With_Category_When_Fetching_All_Products_Then_Return_All_Products_Filtered()
    {
        var category = Guid.NewGuid().ToString();
        await SeedDatabase(category);
        var query = new GetAllProductsQuery(category);
        var handler = new GetAllProductsHandler(_database);
        var result = await handler.Handle(query, CancellationToken.None);
        result.Should().NotBeNull();
        result.Should().HaveCount(500);
        result[0].Should().BeOfType<GetAllProductsResults>();
        foreach (var product in result)
        {
            product.ProductCategory.Should().Be(category);
        }
    }

    [Fact]
    public async Task Given_All_Products_With_Non_Existent_Category_When_Fetching_All_Products_Then_Return_No_Products()
    {
        var category = Guid.NewGuid().ToString();
        await SeedDatabase(); // random categories
        var query = new GetAllProductsQuery(category); //queries random category
        var handler = new GetAllProductsHandler(_database);
        var result = await handler.Handle(query, CancellationToken.None);
        result.Should().BeEmpty();
    }
}
