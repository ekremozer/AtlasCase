using System.Text;
using System.Text.Json;
using AtlasCase.Api.Services;
using AtlasCase.Service.Model;
using AtlasCase.Service.Orders;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AtlasCase.Api.BackgroundServices
{
    public class OrderBackgroundService : BackgroundService
    {
        private readonly RabbitMQClientService _rabbitMqClientService;
        private IModel _channel;
        private readonly IOrderService _orderService;

        public OrderBackgroundService(RabbitMQClientService rabbitMqClientService, IOrderService orderService)
        {
            _rabbitMqClientService = rabbitMqClientService;
            _orderService = orderService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMqClientService.Connect();
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region OrderConsumer
            var orderConsumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: RabbitMQClientService.OrderQueueName, autoAck: false, consumer: orderConsumer);
            orderConsumer.Received += OrderConsumer_Received;
            #endregion

            #region BulkOrderConsumer
            var bulkOrderConsumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: RabbitMQClientService.BulkOrderQueueName, autoAck: false, consumer: bulkOrderConsumer);
            bulkOrderConsumer.Received += BulkOrderConsumer_Received;
            #endregion

            return Task.CompletedTask;
        }

        private Task OrderConsumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var bodyArray = @event.Body.ToArray();
                var bodyString = Encoding.UTF8.GetString(bodyArray);
                var order = JsonSerializer.Deserialize<SaveOrderModel>(bodyString);
                if (order != null)
                {
                    var response = _orderService.SaveOrder(order).Result;

                    _channel.BasicAck(@event.DeliveryTag, false);

                    //TODO:Send response to Customer
                }
                else
                {
                    //TODO:Failed Log
                }

            }
            catch (Exception e)
            {
                //TODO:Failed Log
            }

            return Task.CompletedTask;
        }

        private Task BulkOrderConsumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var bodyArray = @event.Body.ToArray();
                var bodyString = Encoding.UTF8.GetString(bodyArray);
                var orderList = JsonSerializer.Deserialize<List<SaveOrderModel>>(bodyString);
                if (orderList != null && orderList.Any())
                {
                    var responses = _orderService.SaveBulkOrder(orderList).Result;

                    _channel.BasicAck(@event.DeliveryTag, false);

                    //TODO:Send responses to Customer
                }
                else
                {
                    //TODO:Failed Log
                }

            }
            catch (Exception e)
            {
                //TODO:Failed Log
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
