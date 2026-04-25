using E_Commerce.Infrastructure.Enums;

namespace E_Commerce.Data.Entities;

public class Users
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public StoreRole Role { get; set; } = StoreRole.Customer;

    public ICollection<Addresses> Addresses { get; set; } = new List<Addresses>();
    public ICollection<Orders> Orders { get; set; } = new List<Orders>();
    public Cart? Cart { get; set; }
    public WishList? WishLists { get; set; }
}
