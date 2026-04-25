using E_Commerce.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Users> Users { get; set; }
    public DbSet<Addresses> Addresses { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<WishList> WishList { get; set; }
    public DbSet<Orders> Orders { get; set; }
}
