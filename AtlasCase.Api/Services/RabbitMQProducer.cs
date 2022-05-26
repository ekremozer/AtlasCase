using System.Text;
using System.Text.Json;
using AtlasCase.Service.Model;

namespace AtlasCase.Api.Services
{
    public class RabbitMQProducer
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQProducer(RabbitMQClientService rabbitMqClientService)
        {
            _rabbitMQClientService = rabbitMqClientService;
        }

        public void PublishOrder(SaveOrderModel order)
        {
            var channel = _rabbitMQClientService.Connect();

            var bodyJson = JsonSerializer.Serialize(order);

            var bodyByte = Encoding.UTF8.GetBytes(bodyJson);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            //channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingOrder, mandatory: true, basicProperties: properties, body: bodyByte);
            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingOrder, mandatory: true, basicProperties: properties, body: bodyByte);            
        }

        public void PublishBulkOrder(List<SaveOrderModel> list)
        {
            var channel = _rabbitMQClientService.Connect();

            var bodyJson = JsonSerializer.Serialize(list);

            var bodyByte = Encoding.UTF8.GetBytes(bodyJson);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingBulkOrder, mandatory: true, basicProperties: properties, body: bodyByte);
        }
    }
}
