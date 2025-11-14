using VolleyballGear.Data.Domain;

namespace VolleyballGear.Core.Contracts
{
    public interface IShoppingCartService
    {
        List<ShoppingCart> GetAll(string userId);
        bool AddToCart(string userId, int productId, int quantity);
        bool RemoveById(string userId, int productId);
        bool CleanCart(string userId);
        bool ChangeQuantity(string userId, int productId, int quantity);
    }
}
