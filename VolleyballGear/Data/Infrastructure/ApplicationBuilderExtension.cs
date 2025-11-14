using Microsoft.AspNetCore.Identity;
using VolleyballGear.Data.Domain;

namespace VolleyballGear.Data.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            await RoleSeeder(services);
            await SeedAdministrator(services);

            var dataCategory = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedCategories(dataCategory);

            var dataBrand = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedBrands(dataBrand);

            return app;
        }

        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Client" };

            IdentityResult roleResult;

            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);

                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        private static async Task SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (await userManager.FindByNameAsync("admin") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "admin";
                user.LastName = "admin";
                user.UserName = "admin";
                user.Email = "admin@admin.com";
                user.PhoneNumber = "0888888888";

                var result = await userManager.CreateAsync
                    (user, "Admin123456");

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        private static void SeedCategories(ApplicationDbContext dataCategory)
        {
            if (dataCategory.Categories.Any())
            {
                return;
            }
            dataCategory.Categories.AddRange(new[]
            {
                new Category {CategoryName="Balls"},
                new Category {CategoryName="Clothes"},
                new Category {CategoryName="Accessories"},
                new Category {CategoryName="Nets"},
                new Category {CategoryName="Bags"},
                new Category {CategoryName="Shoes"},

            });
            dataCategory.SaveChanges();
        }
        private static void SeedBrands(ApplicationDbContext dataBrands)
        {
            if (dataBrands.Brands.Any())
            {
                return;
            }
            dataBrands.Brands.AddRange(new[]
            {
                new Brand {BrandName="Mikasa"},
                new Brand {BrandName="Adidas"},
                new Brand {BrandName="ASICS"},
                new Brand {BrandName="Fila"},
                new Brand {BrandName="Nike"},
                new Brand {BrandName="Puma"},
                new Brand {BrandName="Mizuno"},
                new Brand {BrandName="Under Armour"},
            });
            dataBrands.SaveChanges();
        }
    }
}
