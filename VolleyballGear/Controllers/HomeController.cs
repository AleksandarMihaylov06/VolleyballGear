using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VolleyballGear.Core.Contracts;
using VolleyballGear.Models;
using VolleyballGear.Models.Product;

namespace VolleyballGear.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }
    public IActionResult Index()
    {
        var topProducts = _productService.TopSoldProducts(3)
            .Select(x => new ProductIndexVM()
            {
                Id = x.Id,
                ProductName = x.ProductName,
                BrandId = x.BrandId,
                BrandName = x.Brand.BrandName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                Picture = x.Picture,
                Quantity = x.Quantity,
                Price = x.Price,
                PriceWithDiscount = (x.Price - x.Price * x.Discount / 100).ToString("f2").ToString(),
                Discount = x.Discount,
                HasDiscount = x.Discount != 0,
            }).ToList();
        return View(topProducts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
