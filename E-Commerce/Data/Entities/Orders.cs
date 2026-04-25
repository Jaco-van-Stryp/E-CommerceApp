using E_Commerce.Infrastructure.Enums;

namespace E_Commerce.Data.Entities;

public class Orders
{
    public Guid Id { get; set; }
    public Users User { get; set; } = null!;
    public ICollection<Products> Products { get; set; } = new List<Products>();
    public DateTime DatePurchased { get; set; } = DateTime.UtcNow;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Processing;
    public required double TotalPaid { get; set; }
}
