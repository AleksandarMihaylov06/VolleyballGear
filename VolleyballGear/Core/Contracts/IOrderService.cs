using VolleyballGear.Data.Domain;
using VolleyballGear.Models.Order;

namespace VolleyballGear.Core.Contracts
{
    public interface IOrderService
    {
        bool Create(string userId, IList<OrderProduct> orderProducts);
        IList<Order> GetOrders();
        IList<Order> GetOrdersByUser(string userId);
        Order GetOrderById(int orderId);

        OrderDetailsVM GetOrderDetails(int id);
        bool CancelOrder(int id);

        bool ChangeOrderStatus(int id, string status);
    }
}
