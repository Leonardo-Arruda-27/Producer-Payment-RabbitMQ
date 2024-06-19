using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Fila_RabbitMQ
{
    public class OrderPublisher
    {
        private readonly IModel _channel;

        public OrderPublisher(IModel channel)
        {
            _channel = channel;
            channel.QueueDeclare(queue: "orderQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void PublishOrder(dynamic order)
        {
            string message = JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: "orderQueue",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
