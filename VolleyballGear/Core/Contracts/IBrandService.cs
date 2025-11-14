using VolleyballGear.Data.Domain;

namespace VolleyballGear.Core.Contracts
{
    public interface IBrandService
    {
        List<Brand> GetBrands();
        Brand GetBrandById(int brandId);
        List<Product> GetProductByBrand(int brandId);
    }
}
