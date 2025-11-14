using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VolleyballGear.Core.Contracts;
using VolleyballGear.Core.Services;
using VolleyballGear.Data;
using VolleyballGear.Data.Domain;
using VolleyballGear.Data.Infrastructure;
using VolleyballGear.Services;

namespace VolleyballGear;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw
            new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies()
            .UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 5;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddTransient<ICategoryService, CategoryService>();
        builder.Services.AddTransient<IBrandService, BrandServise>();
        builder.Services.AddTransient<IProductService, ProductService>();
        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();

        var app = builder.Build();

        app.PrepareDatabase();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
