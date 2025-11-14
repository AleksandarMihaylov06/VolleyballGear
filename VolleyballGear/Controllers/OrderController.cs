using VolleyballGear.Core.Contracts;
using VolleyballGear.Data.Domain;
using VolleyballGear.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace LaptopShopApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _cartService;

        public OrderController(IProductService productService, IOrderService orderService, IShoppingCartService cartService)
        {
            _productService = productService;
            _orderService = orderService;
            _cartService = cartService;
        }

        // GET: OrderController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var orders = _orderService.GetOrders()
                .Select(x => new OrderIndexVM
                {
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    OrderDate = x.OrderDate,
                    OrderId = x.Id,
                    TotalPrice = x.OrderProducts
                        .Select(p => (p.Product.Price - p.Product.Price * p.Product.Discount / 100) * p.Quantity)
                        .Sum(),
                    Status = x.Status,
                    OrderProducts = x.OrderProducts.Select(p => new OrderProductIndexVM
                    {
                        ProductId = p.ProductId,
                        ProductName = p.Product.ProductName,
                        Pic = p.Product.Picture,
                        Quantity = p.Quantity,
                        Price = (p.Product.Price - p.Product.Price * p.Product.Discount / 100)
                    }).ToList()
                })
                .OrderByDescending(x => x.OrderId)
                .ToList();

            return View(orders);
        }

        // GET: OrderController/Create
        public ActionResult Checkout()
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orderCreate = new OrderCreateVM()
            {
                OrderProducts = _cartService.GetAll(currentUserId)
                .Select(x => new CreateOrderProductVM()
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.ProductName,
                    Pic = x.Product.Picture,
                    Quantity = x.Quantity,
                    Price = x.Product.Price.ToString("f2"),
                    Discount = x.Product.Discount
                }).ToList()
            };

            orderCreate.TotalPrice = orderCreate.OrderProducts.Select(x => decimal.Parse(x.Price) * x.Quantity).Sum();

            return View(orderCreate);
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(OrderCreateVM orderCreate)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var created = _orderService.Create(
                    currentUserId,
                    orderCreate.OrderProducts.Select(x => new OrderProduct
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = decimal.Parse(x.Price),
                        Discount = x.Discount
                    }).ToList()
                );

                if (created)
                {
                    foreach (var item in orderCreate.OrderProducts)
                    {
                        var product = _productService.GetProductById(item.ProductId);
                        if (product != null)
                        {
                            product.Quantity -= item.Quantity;
                            _productService.UpdateProduct(product);
                        }
                    }
                    _cartService.CleanCart(currentUserId);
                    return RedirectToAction(nameof(MyOrders));
                }
            }
            return View(orderCreate);
        }
        public ActionResult MyOrders()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = _orderService.GetOrdersByUser(currentUserId)
                .Select(x => new OrderIndexVM
                {
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    OrderDate = x.OrderDate,
                    OrderId = x.Id,
                    TotalPrice = x.OrderProducts
                        .Select(p => (p.Product.Price - p.Product.Price * p.Product.Discount / 100) * p.Quantity)
                        .Sum(),
                    Status = x.Status, 
                    OrderProducts = x.OrderProducts.Select(p => new OrderProductIndexVM
                    {
                        ProductId = p.ProductId,
                        ProductName = p.Product.ProductName,
                        Pic = p.Product.Picture,
                        Quantity = p.Quantity,
                        Price = (p.Product.Price - p.Product.Price * p.Product.Discount / 100)
                    }).ToList()
                })
                .OrderByDescending(x => x.OrderId)
                .ToList();

            return View(orders);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            // Restrict access: user can only see their own orders, unless admin
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Administrator") && order.UserId != currentUserId)
            {
                return Forbid();
            }

            // Build the view model
            var orderDetails = new OrderDetailsVM
            {
                OrderId = order.Id,
                UserEmail = order.User.Email,
                OrderDate = order.OrderDate,
                Status = order.Status, // ✅ Add this line
                OrderProducts = order.OrderProducts.Select(p => new OrderProductIndexVM
                {
                    ProductId = p.ProductId,
                    ProductName = p.Product.ProductName,
                    Pic = p.Product.Picture,
                    Quantity = p.Quantity,
                    Price = (p.Product.Price - p.Product.Price * p.Product.Discount / 100)
                }).ToList()
            };

            // Calculate total price
            orderDetails.TotalPrice = orderDetails.OrderProducts
                .Select(p => p.Price * p.Quantity)
                .Sum();

            return View(orderDetails);
        }
        // 🔹 GET: Order/Denied/{id}
        [Authorize(Roles = "Administrator")]
        public ActionResult ManageStatus(int id)
        {
            var order = _orderService.GetOrderDetails(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // 🔹 POST: Order/Cancel/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            bool success = _orderService.CancelOrder(id);

            if (!success)
            {
                TempData["ErrorMessage"] = "Order could not be cancelled or was already cancelled.";
                return RedirectToAction("MyOrders");
            }

            TempData["SuccessMessage"] = "Your order has been successfully cancelled.";
            return RedirectToAction("MyOrders");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(int id, string status)
        {
            bool success = _orderService.ChangeOrderStatus(id, status);

            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to change order status.";
            }
            else
            {
                TempData["SuccessMessage"] = "Order status successfully updated.";
            }

            return RedirectToAction("ManageStatus", new { id });
        }
    }
}
