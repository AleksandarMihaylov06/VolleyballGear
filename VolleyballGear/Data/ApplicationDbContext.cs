using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VolleyballGear.Data.Domain;

namespace VolleyballGear.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ShoppingCart>().HasKey(x => new { x.UserId, x.ProductId });
        base.OnModelCreating(builder);
    }
}
