using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Core;
using AtlasCase.Data.Entities;
using AtlasCase.Service.Model;
using Microsoft.EntityFrameworkCore;

namespace AtlasCase.Service.Orders;

public class OrderService : IOrderService
{
    private readonly IContextFactory _contextFactory;
    public OrderService(IContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Order?> GetOrder(int id)
    {
        await using var context = _contextFactory.Create();
        return await context.Orders.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<OrderDto>> GetOrders()
    {
        await using var context = _contextFactory.Create();
        var orderList = await context.Orders.Select(x => new OrderDto
        {
            Id = x.Id,
            OrderNo = x.OrderNo,
            CustomerOrderNo = x.CustomerOrderNo,
            PortAddress = x.PortAddress,
            ArrivalAddress = x.ArrivalAddress,
            Quantity = x.Quantity,
            UnitId = x.UnitId,
            Unit = x.Unit.Name,
            Weight = x.Weight,
            WeightUnitId = x.WeightUnitId,
            WeightUnit = x.WeightUnit.Name,
            MaterialId = x.MaterialId,
            MaterialCode = x.Material.Code,
            MaterialName = x.Material.Name,
            Note = x.Note,
            OrderStatusId = x.OrderStatusId,
            OrderStatus = x.OrderStatus.Name
        }).ToListAsync();
        return orderList;
    }

    public async Task<UpdateOrderStatusResponse> UpdateOrderStatus(OrderStatusModel model)
    {
        await using var context = _contextFactory.Create();
        var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == model.OrderId);

        #region Validation
        if (order == null)
        {
            return new UpdateOrderStatusResponse
            {
                Status = 0,
                Message = "Sipariş bulunamadı."
            };
        }

        var orderStatus = await context.OrderStatuses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.StatusId);
        if (orderStatus == null)
        {
            return new UpdateOrderStatusResponse
            {
                Status = 0,
                Message = "Hatalı durum gönderdiniz"
            };
        }

        if (order.OrderStatusId == model.StatusId)
        {
            return new UpdateOrderStatusResponse
            {
                Status = 0,
                Message = "Farklı bir sipariş durumu göndermelisiniz."
            };
        }
        #endregion

        order.OrderStatusId = model.StatusId;
        order.OrderStatusUpdateAt = DateTime.Now;
        await context.SaveChangesAsync();

        return new UpdateOrderStatusResponse
        {
            CustomerOrderNo = order.CustomerOrderNo,
            Status = 1,
            Message = $"Sipariş durumu {orderStatus.Name} olarak güncellendi.",
            OrderStatusUpdateAt = order.OrderStatusUpdateAt,
            OrderStatus = orderStatus.Name
        };
    }

    public async Task<List<OrderStatusDto>> GetOrderStatuses()
    {
        await using var context = _contextFactory.Create();
        return await context.OrderStatuses.Select(x => new OrderStatusDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();
    }

    public async Task<SaveOrderResponse> SaveOrder(SaveOrderModel model)
    {
        await using var context = _contextFactory.Create();

        #region Validation
        var customerOrderNoAny = context.Orders.Any(x => x.CustomerOrderNo == model.CustomerOrderNo);
        if (customerOrderNoAny)
        {
            return new SaveOrderResponse
            {
                Status = 0,
                Message = "Bu müşteri sipariş numarası daha önce kullanılmış."
            };
        }

        var unitValidation = context.Units.Any(x => x.Id == model.UnitId);
        if (unitValidation == false)
        {
            return new SaveOrderResponse
            {
                Status = 0,
                Message = "Hatalı miktar birimi gönderdiniz."
            };
        }

        var weightUnitValidation = context.WeightUnits.Any(x => x.Id == model.WeightUnitId);
        if (weightUnitValidation == false)
        {
            return new SaveOrderResponse
            {
                Status = 0,
                Message = "Hatalı ağırlık birimi gönderdiniz."
            };
        }
        #endregion

        #region GetOrCreateMaterial
        var material = context.Materials.FirstOrDefault(x => x.Code == model.MaterialCode);
        if (material == null)
        {
            material = new Material
            {
                Name = model.MaterialName,
                Code = model.MaterialCode
            };
            context.Materials.Add(material);
            await context.SaveChangesAsync();
        }
        #endregion

        #region OrderNoGeneration
        var orderNo = DateTime.Now.ToString("yyMMddHHmm");
        var orderCount = await context.Orders.CountAsync(x => x.CreatedAt.Date == DateTime.Now.Date);
        orderNo += (orderCount + 1).ToString("D4");

        var orderNoAny = context.Orders.Any(x => x.OrderNo == orderNo);
        while (orderNoAny)
        {
            orderCount++;
            orderNo = DateTime.Now.ToString("yyMMddHHmm");
            orderNo += (orderCount).ToString("D4");
            orderNoAny = context.Orders.Any(x => x.OrderNo == orderNo);
        }

        #endregion

        #region SaveOrder
        var order = new Order
        {
            OrderNo = orderNo,
            CustomerOrderNo = model.CustomerOrderNo,
            PortAddress = model.PortAddress,
            ArrivalAddress = model.ArrivalAddress,
            Quantity = model.Quantity,
            UnitId = model.UnitId,
            Weight = model.Weight,
            WeightUnitId = model.WeightUnitId,
            MaterialId = material.Id,
            Note = model.Note,
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();
        #endregion

        #region ReturnModel
        return new SaveOrderResponse
        {
            Status = 1,
            Message = "Sipariş kaydedildi.",
            CustomerOrderNo = order.CustomerOrderNo,
            OrderNo = order.OrderNo
        };
        #endregion
    }

    public async Task<List<SaveOrderResponse>> SaveBulkOrder(List<SaveOrderModel> list)
    {
        var responseList = new List<SaveOrderResponse>();

        foreach (var item in list)
        {
            responseList.Add(await SaveOrder(item));
        }

        return responseList;
    }
}