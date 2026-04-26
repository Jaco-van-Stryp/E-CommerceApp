using E_Commerce.Data;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceTests.Factory;

public static class DbContextFactory
{
    public static AppDbContext CreateDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
}
