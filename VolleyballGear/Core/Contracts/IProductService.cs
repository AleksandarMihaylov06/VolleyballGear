using VolleyballGear.Data.Domain;

namespace VolleyballGear.Core.Contracts
{
    public interface IProductService
    {
        int CountProducts();
        int CountOrders();
        decimal SumOrders();



        bool Create(string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount);
        bool Update(int productId, string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount);

        List<Product> GetProducts();
        Product GetProductById(int productId);

        bool RemoveById(int dogproductId);
        List<Product> GetProducts(string searchStringCategoryName, string searchStringBrandName, string searchStringProductName);
        void UpdateProduct(Product product);
        IList<Product> TopSoldProducts(int productCount);
    }
}
