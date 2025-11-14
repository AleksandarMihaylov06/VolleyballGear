using VolleyballGear.Core.Contracts;
using VolleyballGear.Data.Domain;
using VolleyballGear.Data;
using Microsoft.EntityFrameworkCore;

namespace VolleyballGear.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public ProductService(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public int CountOrders()
        {
            return _context.Orders.Count();
        }
        public int CountProducts()
        {
            return _context.Products.Count();
        }
        public decimal SumOrders()
        {
            var sum = _context.OrderProducts.Sum(x => x.Quantity * x.Price - x.Quantity * x.Price * x.Discount / 100);

            return sum;
        }


        public bool Create(string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount)
        {
            Product item = new Product
            {
                ProductName = name,
                Brand = _context.Brands.Find(brandId),
                Category = _context.Categories.Find(categoryId),
                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount
            };

            _context.Products.Add(item);
            return _context.SaveChanges() != 0;
        }

        public List<Product> GetMostExpensive()
        {
            return _context.Products
                .Where(x => (x.Price - (x.Price * x.Discount / 100)) > 1000)
                .OrderBy(x => x.Price - (x.Price * x.Discount / 100))
                .ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public List<Product> GetProducts()
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }

        public List<Product> GetProducts(string searchStringCategoryName, string searchStringBrandName, string searchStringProductName)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStringCategoryName))
            {
                query = query.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchStringBrandName))
            {
                query = query.Where(x => x.Brand.BrandName.ToLower().Contains(searchStringBrandName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchStringProductName))
            {
                query = query.Where(x => x.ProductName.ToLower().Contains(searchStringProductName.ToLower()));
            }

            return query.ToList();
        }

        public List<Product> GetPromo()
        {
            return _context.Products
                .Where(x => x.Discount > 0)
                .ToList();
        }

        public bool RemoveById(int productId)
        {
            var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }
            _context.Remove(product);
            return _context.SaveChanges() != 0;
        }

        public IList<Product> TopSoldProducts(int productCount)
        {
            var productSales = new Dictionary<Product, int>();

            var allOrders = _orderService.GetOrders();

            foreach (var order in allOrders)
            {
                foreach (var item in order.OrderProducts)
                {
                    if (productSales.ContainsKey(item.Product))
                    {
                        productSales[item.Product] += item.Quantity;
                    }
                    else
                    {
                        productSales[item.Product] = item.Quantity;
                    }
                }
            }

            return productSales
                .OrderByDescending(p => p.Value)
                .Take(productCount)
                .Select(p => p.Key)
                .ToList();
        }

        public bool Update(int productId, string name, int brandId, int categoryId, string picture, int quantity, decimal price, decimal discount)
        {
            var products = GetProductById(productId);
            if (products == default(Product))
            {
                return false;
            }
            products.ProductName = name;
            //products.BrandId = brandId;
            //products.CategoryId = categoryId;

            products.Brand = _context.Brands.Find(brandId);
            products.Category = _context.Categories.Find(categoryId);
            products.Picture = picture;
            products.Quantity = quantity;
            products.Price = price;
            products.Discount = discount;
            _context.Update(products);
            return _context.SaveChanges() != 0;
        }
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
