namespace E_Commerce.Data.Entities;

public class Products
{
    public Guid Id { get; set; }
    public required string ProductName { get; set; }
    public required string ProductDescription { get; set; }
    public required decimal ProductPrice { get; set; }
    public required Guid ProductImageFileName { get; set; }
    public required string? ProductCategory { get; set; }
    public bool Active { get; set; } = true;
}
