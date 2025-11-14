using VolleyballGear.Data.Domain;

namespace VolleyballGear.Core.Contracts
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        Category GetCategoryById(int categoryId);
        List<Product> GetProductByCategory(int categoryId);
    }
}
