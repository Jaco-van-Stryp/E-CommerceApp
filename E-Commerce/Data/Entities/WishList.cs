namespace E_Commerce.Data.Entities;

public class WishList
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Users User { get; set; } = null!;
    public required ICollection<Products> Products { get; set; } = new List<Products>();
}
