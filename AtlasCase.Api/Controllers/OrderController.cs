
using AtlasCase.Api.Services;
using AtlasCase.Data.Core;
using AtlasCase.Service.Model;
using AtlasCase.Service.Orders;
using Microsoft.AspNetCore.Mvc;

namespace AtlasCase.Api.Controllers
{
    [ApiController]
    [Route("Order")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly string _customerApiUrl;
        private readonly RabbitMQProducer _rabbitMQProducer;
        private readonly IContextFactory _contextFactory;
        public OrderController(IOrderService orderService, IConfiguration configuration, RabbitMQProducer rabbitMqProducer, IContextFactory contextFactory)
        {
            _orderService = orderService;
            _rabbitMQProducer = rabbitMqProducer;
            _contextFactory = contextFactory;
            _customerApiUrl = configuration["CustomerApiUrl"];
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var orderList = await _orderService.GetOrders();
            return Json(orderList);
        }

        [HttpPost]
        [Route("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(OrderStatusModel model)
        {
            var result = await _orderService.UpdateOrderStatus(model);
            if (result.Status == 1)
            {
                var postModel = new
                {
                    result.CustomerOrderNo,
                    Status = result.OrderStatus,
                    UpdateAt = result.OrderStatusUpdateAt

                };
                HttpService.PostAsync(_customerApiUrl, postModel);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("GetOrderStatuses")]
        public async Task<IActionResult> GetOrderStatuses()
        {
            var statusList = await _orderService.GetOrderStatuses();
            return Json(statusList);
        }

        [HttpPost]
        [Route("SaveOrder")]
        public async Task<IActionResult> SaveOrder(SaveOrderModel model)
        {
            var response = await _orderService.SaveOrder(model);
            return Json(response);
        }

        [HttpPost]
        [Route("SaveBulkOrder")]
        public async Task<IActionResult> SaveBulkOrder(List<SaveOrderModel> list)
        {
            var responses = await _orderService.SaveBulkOrder(list);
            return Json(responses);
        }

        [HttpPost]
        [Route("SaveOrderAsync")]
        public async Task<IActionResult> SaveOrderAsync(SaveOrderModel model)
        {
            _rabbitMQProducer.PublishOrder(model);
            //var response = await _orderService.SaveOrder(model);
            return Json(new { Stastus = 1, Message = "Siparişiniz işleme alındı" });
        }

        [HttpPost]
        [Route("SaveBulkOrderAsync")]
        public async Task<IActionResult> SaveBulkOrderAsync(List<SaveOrderModel> list)
        {
            _rabbitMQProducer.PublishBulkOrder(list);
            //var responses = await _orderService.SaveBulkOrder(list);
            return Json(new { Stastus = 1, Message = "Siparişleriniz işleme alındı" });
        }

        [HttpGet]
        [Route("Install")]
        public Task<IActionResult> Install()
        {
            var context = _contextFactory.Create();
            context.DbCreator();
            return Task.FromResult<IActionResult>(Json("ok"));
        }
    }
}
