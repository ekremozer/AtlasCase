using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Entities;
using AtlasCase.Service.Model;

namespace AtlasCase.Service.Orders;

public interface IOrderService
{
    Task<Order?> GetOrder(int id);
    Task<List<OrderDto>> GetOrders();
    Task<List<OrderStatusDto>> GetOrderStatuses();
    Task<UpdateOrderStatusResponse> UpdateOrderStatus(OrderStatusModel model);
    Task<SaveOrderResponse> SaveOrder(SaveOrderModel model);
    Task<List<SaveOrderResponse>> SaveBulkOrder(List<SaveOrderModel> list);
}