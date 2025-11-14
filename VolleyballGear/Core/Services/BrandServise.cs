using VolleyballGear.Core.Contracts;
using VolleyballGear.Data.Domain;
using VolleyballGear.Data;

namespace VolleyballGear.Core.Services
{
    public class BrandServise : IBrandService
    {
        private readonly ApplicationDbContext _context;

        public BrandServise(ApplicationDbContext context)
        {
            _context = context;
        }

        public Brand GetBrandById(int brandId)
        {
            return _context.Brands.Find(brandId);
        }

        public List<Brand> GetBrands()
        {
            List<Brand> brands = _context.Brands.ToList();
            return brands;
        }

        public List<Product> GetProductByBrand(int brandId)
        {
            return _context.Products
                 .Where(x => x.BrandId == brandId)
                 .ToList();
        }
    }
}
