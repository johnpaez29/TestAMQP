using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace AMQPTestReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientSend = Convert.ToString(args[0]);

            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using var channel = connection.CreateModel();
                channel.QueueDeclare(clientSend, false, false, false, null);

                var receiveClient = new EventingBasicConsumer(channel);

                receiveClient.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine(message);
                };

                channel.BasicConsume(clientSend, true, receiveClient);

                Console.WriteLine("esperando ......");
                Console.ReadLine();

            }
        }
    }
}
