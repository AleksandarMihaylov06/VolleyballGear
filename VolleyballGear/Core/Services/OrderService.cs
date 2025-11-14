using VolleyballGear.Data.Domain;
using VolleyballGear.Data;
using VolleyballGear.Core.Contracts;
using VolleyballGear.Models.Order;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace VolleyballGear.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(string userId, IList<OrderProduct> orderProducts)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return false;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderProducts = orderProducts
            };

            _context.Orders.Add(order);

            foreach (var orderProduct in orderProducts)
            {
                orderProduct.OrderId = order.Id;
                _context.OrderProducts.Add(orderProduct);
            }

            return _context.SaveChanges() > 0;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(p => p.Product)
                .Include(o => o.User)
                .FirstOrDefault(x => x.Id == orderId)!;
        }

        public IList<Order> GetOrders()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(p => p.Product)
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }

        public IList<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(p => p.Product)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }

        public OrderDetailsVM GetOrderDetails(int id)
        {
            var order = _context.Orders
         .Where(o => o.Id == id)
         .Select(o => new OrderDetailsVM
         {
             OrderId = o.Id,
             UserEmail = o.User.Email,
             OrderDate = o.OrderDate,
             Status = o.Status,
             TotalPrice = o.OrderProducts
                 .Select(p => (p.Product.Price - p.Product.Price * p.Product.Discount / 100) * p.Quantity)
                 .Sum(),
             OrderProducts = o.OrderProducts.Select(p => new OrderProductIndexVM
             {
                 ProductId = p.ProductId,
                 ProductName = p.Product.ProductName,
                 Quantity = p.Quantity,
                 Price = (p.Product.Price - p.Product.Price * p.Product.Discount / 100)
             }).ToList()
         })
         .FirstOrDefault();

            return order;
        }

        public bool CancelOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null || order.Status == OrderStatus.Cancelled)
                return false;

            order.Status = OrderStatus.Cancelled;
            _context.SaveChanges();
            return true;
        }

        public bool ChangeOrderStatus(int id, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return false;

            if (!Enum.TryParse<OrderStatus>(status, out var newStatus))
                return false;

            if (order.Status != newStatus)
            {
                order.Status = newStatus;
                _context.SaveChanges();
            }

            return true;
        }
    }
}
