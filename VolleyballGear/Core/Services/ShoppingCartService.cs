using Microsoft.EntityFrameworkCore;
using VolleyballGear.Core.Contracts;
using VolleyballGear.Data;
using VolleyballGear.Data.Domain;

namespace VolleyballGear.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public ShoppingCartService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public bool AddToCart(string userId, int productId, int quantity)
        {
            if (productId == null)
            {
                return false;
            }

            Product? product = _context.Products.FirstOrDefault(x => x.Id == productId);

            if (product?.Quantity <= 0)
            {
                return false;
            }

            ShoppingCart? cart = _context.ShoppingCarts.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);
            if (quantity <= 0)
            {
                return false;
            }

            if (cart != null)
            {
                return ChangeQuantity(userId, productId, quantity);
            }

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };

            _context.ShoppingCarts.Add(shoppingCart);
            return _context.SaveChanges() != 0;
        }

        public bool ChangeQuantity(string userId, int productId, int quantity)
        {
            // IMPORTANT FIX: Include Product to access Product.Quantity
            ShoppingCart? cart = _context.ShoppingCarts
                .Include(x => x.Product) // ADDED THIS
                .FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);

            if (cart == null)
            {
                return false;
            }

            cart.Quantity += quantity;

            // Check if quantity is valid
            if (cart.Quantity <= 0 || cart.Product.Quantity < cart.Quantity)
            {
                return false;
            }

            _context.ShoppingCarts.Update(cart);
            return _context.SaveChanges() != 0;
        }

        public bool CleanCart(string userId)
        {
            List<ShoppingCart> userCartItems = _context.ShoppingCarts
                .Where(x => x.UserId == userId)
                .ToList();

            if (!userCartItems.Any())
            {
                return false;
            }

            _context.ShoppingCarts.RemoveRange(userCartItems);
            return _context.SaveChanges() != 0;
        }

        public List<ShoppingCart> GetAll(string userId)
        {
            // IMPORTANT FIX: Include Product to access Product properties in the view
            List<ShoppingCart> carts = _context.ShoppingCarts
                .Include(x => x.Product) // ADDED THIS
                .Where(x => x.UserId == userId)
                .ToList();

            return carts;
        }

        public bool RemoveById(string userId, int productId)
        {
            ShoppingCart? cart = _context.ShoppingCarts
                .FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);

            if (cart == null)
            {
                return false;
            }

            _context.ShoppingCarts.Remove(cart);
            return _context.SaveChanges() != 0;
        }
    }
}