using RabbitMQ.Client;

namespace AtlasCase.Api.Services
{
    public class RabbitMQClientService : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        public static string ExchangeName = "OrderDirectExchange";
        public static string RoutingOrder = "order-route";
        public static string RoutingBulkOrder = "bulk-order-route";
        public static string OrderQueueName = "order-queue";
        public static string BulkOrderQueueName = "bulk-order-queue";

        public RabbitMQClientService(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false);
            
            _channel.QueueDeclare(queue: OrderQueueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: OrderQueueName, exchange: ExchangeName, routingKey: RoutingOrder);

            _channel.QueueDeclare(queue: BulkOrderQueueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: BulkOrderQueueName, exchange: ExchangeName, routingKey: RoutingBulkOrder);

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _channel = default;

            _connection?.Close();
            _connection?.Dispose();
            _connection = default;
        }
    }
}
