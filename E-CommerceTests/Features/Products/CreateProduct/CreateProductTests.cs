using E_Commerce.Data;
using E_Commerce.Features.Products.CreateProduct;
using E_CommerceTests.Factory;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceTests.Features.Products.CreateProduct;

public class CreateProductTests
{
    private const string ProductName = "Samsung S22 Ultra Phone";
    private const string ProductDescription = "A very cool phone";
    private const decimal ProductPrice = 2000;
    private const string ProductCategory = "Mobile Phones";
    private const string FilePreFix = "TODO\\";
    private static readonly Guid ProductImageName = Guid.NewGuid();
    private readonly AppDbContext _database = DbContextFactory.CreateDatabase();

    [Fact]
    public async Task Given_CreateProduct_When_EnteringNewProductDetails_Then_ReturnNewlyCreatedProduct()
    {
        var command = new CreateProductCommand(
            ProductName: ProductName,
            ProductDescription: ProductDescription,
            ProductPrice: ProductPrice,
            ProductCategory: ProductCategory,
            ProductImage: ProductImageName
        );

        var handler = new CreateProductHandler(_database);

        var results = await handler.Handle(command, CancellationToken.None);

        var dataEntry = await _database.Products.FirstOrDefaultAsync(x => x.Id == results.Id);
        dataEntry.Should().NotBeNull();
        dataEntry.Id.Should().Be(results.Id);
        dataEntry.ProductName.Should().Be(command.ProductName);
        dataEntry.ProductDescription.Should().Be(command.ProductDescription);
        dataEntry.ProductPrice.Should().Be(command.ProductPrice);
        dataEntry.ProductCategory.Should().Be(command.ProductCategory);
        dataEntry.Active.Should().BeTrue();
        dataEntry.ProductImageFileName.Should().Be(ProductImageName);
        results.Id.Should().NotBeEmpty();
        results.ProductName.Should().Be(ProductName);
        results.ProductDescription.Should().Be(ProductDescription);
        results.ProductPrice.Should().Be(ProductPrice);
        results.ProductCategory.Should().Be(ProductCategory);
        results.ProductImageFileName.Should().Be(FilePreFix + command.ProductImage);
    }
}
