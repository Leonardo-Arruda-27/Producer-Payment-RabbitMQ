using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using Fila_RabbitMQ;
using RabbitMQ.Client;

class Publisher
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {

            var orderPublisher = new OrderPublisher(channel);

            while (true)
            {
                var order = new
                {
                    userId = "12345",
                    userName = "John Doe",
                    email = "john.doe@example.com",
                    order = new
                    {
                        orderId = "67890",
                        totalAmount = 150.00,
                        currency = "USD",
                        items = new[]
                        {
                            new { itemId = "1", itemName = "Product 1", quantity = 2, price = 50.00 },
                            new { itemId = "2", itemName = "Product 2", quantity = 1, price = 50.00 }
                        }
                    },
                    payment = new
                    {
                        cardNumber = "4111111111111111",
                        expiryDate = "12/23",
                        cvv = "123"
                    }
                };

                orderPublisher.PublishOrder(order);

                // Espera 5 segundos antes de enviar a próxima mensagem
                Thread.Sleep(5000);
            }
        }
    }
}
